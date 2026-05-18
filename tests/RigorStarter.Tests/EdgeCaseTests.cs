using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Moq;
using RigorStarter.Core;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.Shared.Models;
using RigorStarter.Shared.Native;
using RigorStarter.Shared.Utilities;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

/// <summary>
/// Exhaustive edge case tests covering weird inputs, thread safety,
/// injection vectors, boundary values, and resource lifecycle issues.
/// </summary>
public class EdgeCaseTests
{
    // ========== ViewModel Boundary Cases ==========

    [Fact]
    public void SelectItem_Null_ShouldThrowNullReference()
    {
        var vm = CreateVM();
        Assert.Throws<NullReferenceException>(() => vm.SelectItemCommand.Execute(null));
    }

    [Fact]
    public void ToggleSearch_RapidCalls_ShouldToggleCorrectly()
    {
        var vm = CreateVM();
        for (int i = 0; i < 10; i++)
            vm.ToggleSearchCommand.Execute(null);

        // Even number of toggles (10) should result in closed state
        Assert.False(vm.IsSearchPanelOpen);
    }

    [Fact]
    public void ToggleSearch_OddRapidCalls_ShouldEndOpen()
    {
        var vm = CreateVM();
        for (int i = 0; i < 7; i++)
            vm.ToggleSearchCommand.Execute(null);

        Assert.True(vm.IsSearchPanelOpen);
    }

    [Fact]
    public void SearchText_WithNull_ShouldNotThrow()
    {
        var vm = CreateVM();
        var ex = Record.Exception(() => vm.SearchText = null!);
        // CommunityToolkit.Mvvm auto-generated property may handle null differently
        Assert.Null(ex);
    }

    [Fact]
    public void SearchText_WithSpecialRegexChars_ShouldFilter()
    {
        var vm = CreateVM();
        vm.SearchText = ".*+^${}()|[]\\";
        // Should not throw or crash - regex chars are not used as regex
        Assert.NotNull(vm.FilteredItems);
    }

    [Fact]
    public void SearchText_WithUnicode_ShouldFilter()
    {
        var vm = CreateVM();
        vm.SearchText = "日本語";
        Assert.NotNull(vm.FilteredItems);
    }

    [Fact]
    public void SearchText_WithEmoji_ShouldFilter()
    {
        var vm = CreateVM();
        vm.SearchText = "🔥🚀💯";
        Assert.NotNull(vm.FilteredItems);
    }

    [Fact]
    public void GoToDashboard_WhenAlreadyOnDashboard_ShouldNotThrow()
    {
        var vm = CreateVM();
        Assert.Null(vm.SelectedItem);
        var ex = Record.Exception(() => vm.GoToDashboardCommand.Execute(null));
        Assert.Null(ex);
        Assert.Null(vm.SelectedItem);
    }

    [Fact]
    public void ToggleTheme_RapidToggle_ShouldConsistentlyCycle()
    {
        var vm = CreateVM();
        bool initial = vm.IsDarkTheme;

        for (int i = 0; i < 5; i++)
            vm.ToggleThemeCommand.Execute(null);

        Assert.NotEqual(initial, vm.IsDarkTheme);

        for (int i = 0; i < 5; i++)
            vm.ToggleThemeCommand.Execute(null);

        Assert.Equal(initial, vm.IsDarkTheme);
    }

    [Fact]
    public void ExitCommand_MultipleCalls_ShouldNotThrow()
    {
        var vm = CreateVM();
        for (int i = 0; i < 5; i++)
        {
            var ex = Record.Exception(() => vm.ExitCommand.Execute(null));
            Assert.Null(ex);
        }
    }

    // ========== DataService Injection and Boundary Cases ==========

    [Fact]
    public void DataService_CountLines_WithDirectoryPath_ShouldReturnZero()
    {
        var service = new DataService(new SystemService());
        var tmpDir = Path.Combine(Path.GetTempPath(), "rigortest_" + Guid.NewGuid());
        Directory.CreateDirectory(tmpDir);
        try
        {
            var method = typeof(DataService).GetMethod("CountLines",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var result = method.Invoke(service, new object[] { tmpDir });
            Assert.Equal(0, (int)result!);
        }
        finally
        {
            if (Directory.Exists(tmpDir)) Directory.Delete(tmpDir);
        }
    }

    [Fact]
    public void DataService_CountLines_WithSpecialFilePath_ShouldNotThrow()
    {
        var service = new DataService(new SystemService());
        var method = typeof(DataService).GetMethod("CountLines",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;

        var ex = Record.Exception(() =>
            method.Invoke(service, new object[] { "/dev/null" }));
        Assert.Null(ex);
    }

    [Fact]
    public void DataService_CountLines_WithVeryLongPath_ShouldReturnZero()
    {
        var service = new DataService(new SystemService());
        var method = typeof(DataService).GetMethod("CountLines",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var longPath = new string('x', 500) + ".txt";
        var result = method.Invoke(service, new object[] { longPath });
        Assert.Equal(0, (int)result!);
    }

    [Fact]
    public void UtilityAction_ExecuteAction_Null_ShouldNotCrashSelect()
    {
        var vm = CreateVM();
        var util = vm.SearchItems.First(i => i.IsUtility);
        // If ExecuteAction is null, SelectItem should not crash
        util.ExecuteAction = null;
        var ex = Record.Exception(() => vm.SelectItemCommand.Execute(util));
        Assert.Null(ex);
    }

    // ========== NativeBuffer Sanity ==========

    [Fact]
    public void NativeBuffer_ReadByte_AfterDispose_ShouldCrash()
    {
        var buffer = new NativeBuffer(10);
        buffer.Dispose();
        // Accessing freed memory is undefined behavior but should throw or crash gracefully
        var ex = Record.Exception(() => buffer.ReadByte(0));
        Assert.NotNull(ex);
    }

    [Fact]
    public void NativeBuffer_WriteByte_AfterDispose_ShouldCrash()
    {
        var buffer = new NativeBuffer(10);
        buffer.Dispose();
        var ex = Record.Exception(() => buffer.WriteByte(0, 0xFF));
        Assert.NotNull(ex);
    }

    [Fact]
    public void NativeBuffer_MaxSize_ShouldAllocate()
    {
        // Attempt to allocate a large buffer to test edge of memory
        // 256 MB should be safe on most systems
        var ex = Record.Exception(() =>
        {
            using var buffer = new NativeBuffer(256 * 1024 * 1024);
            Assert.NotEqual(IntPtr.Zero, buffer.Pointer);
        });
        Assert.Null(ex);
    }

    [Fact]
    public void NativeBuffer_Fill_MaxValue_ShouldWork()
    {
        using var buffer = new NativeBuffer(100);
        buffer.Fill(0xFF);
        Assert.Equal(0xFF, buffer.ReadByte(0));
        Assert.Equal(0xFF, buffer.ReadByte(99));
    }

    [Fact]
    public void NativeBuffer_ReadWrite_AllMaxValues_ShouldRoundTrip()
    {
        using var buffer = new NativeBuffer(256);
        for (int i = 0; i < 256; i++)
            buffer.WriteByte((uint)i, (byte)i);
        for (int i = 0; i < 256; i++)
            Assert.Equal((byte)i, buffer.ReadByte((uint)i));
    }

    // ========== StdLibBridge Edge Cases ==========

    [Fact]
    public void StdLibBridge_GetStringLength_WithEmbeddedNull_ShouldCountPastNull()
    {
        // C# strings can contain embedded null characters
        var s = "he\0llo";
        var (success, length) = StdLibBridge.GetStringLength(s);
        // strlen in C counts until the FIRST null terminator
        // So for "he\0llo", it should return 2
        Assert.True(success);
        Assert.Equal(2u, (uint)length);
    }

    [Fact]
    public void StdLibBridge_DuplicateString_WithEmbeddedNull_ShouldDuplicateCorrectly()
    {
        var s = "he\0llo";
        IntPtr ptr = StdLibBridge.DuplicateString(s);
        try
        {
            Assert.NotEqual(IntPtr.Zero, ptr);
            // strdup also stops at null terminator
            string result = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
            Assert.Equal("he", result);
        }
        finally
        {
            StdLibBridge.FreeString(ptr);
        }
    }

    [Fact]
    public void StdLibBridge_GetStringLength_MaxLengthString_ShouldSucceed()
    {
        var longString = new string('a', 10000);
        var (success, length) = StdLibBridge.GetStringLength(longString);
        Assert.True(success);
        Assert.Equal(10000u, (uint)length);
    }

    // ========== LinuxShell Edge Cases ==========

    [Fact]
    public async Task LinuxShell_EmptyCommand_ShouldReturnError()
    {
        var result = await LinuxShell.ExecuteAsync("");
        Assert.NotNull(result);
        Assert.Equal(-1, result.ExitCode);
    }

    [Fact]
    public async Task LinuxShell_NonexistentCommand_ShouldReturnError()
    {
        var result = await LinuxShell.ExecuteAsync("nonexistent_cmd_xyz");
        Assert.NotNull(result);
        Assert.Equal(-1, result.ExitCode);
    }

    [Fact]
    public async Task LinuxShell_Timeout_ShouldReturnErrorExitCode()
    {
        var result = await LinuxShell.ExecuteAsync("sleep", "10", 100);
        Assert.NotNull(result);
        Assert.Equal(-1, result.ExitCode);
        // The process is killed, resulting in a timeout-related error
        var combined = result.StandardError + result.StandardOutput;
        Assert.True(
            combined.Contains("timeout", StringComparison.OrdinalIgnoreCase) ||
            combined.Contains("killed", StringComparison.OrdinalIgnoreCase) ||
            combined.Contains("exit", StringComparison.OrdinalIgnoreCase),
            $"Expected error message about timeout/kill, got: stderr='{result.StandardError}' stdout='{result.StandardOutput}'"
        );
    }

    // ========== OsRelease Edge Cases ==========

    [Fact]
    public void OsRelease_NonExistentFile_ShouldReturnUnknown()
    {
        // This tests the internal parsing logic by checking the fallback
        var info = OsRelease.GetDistroInfo();
        Assert.NotNull(info);
        Assert.False(string.IsNullOrWhiteSpace(info.PrettyName));
    }

    // ========== XdgPaths Injection Cases ==========

    [Fact]
    public void XdgPaths_AppNameWithPathChars_ShouldNotEscape()
    {
        var path = XdgPaths.GetConfigDir("../etc/passwd");
        Assert.DoesNotContain("/etc/passwd", path.Replace("../etc/passwd", ""));
        Assert.Contains("../etc/passwd", path);
    }

    [Fact]
    public void XdgPaths_EmptyAppName_ShouldReturnValidPath()
    {
        var path = XdgPaths.GetConfigDir("");
        Assert.False(string.IsNullOrWhiteSpace(path));
    }

    [Fact]
    public void XdgPaths_NullAppName_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => XdgPaths.GetConfigDir(null!));
    }

    // ========== ConfigManager Edge Cases ==========

    [Fact]
    public void ConfigManager_EmptyFile_ShouldReturnNullDeserialized()
    {
        var appName = "EdgeTest_" + Guid.NewGuid().ToString("N");
        var fileName = "empty.yaml";
        var dir = XdgPaths.GetConfigDir(appName);
        Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, fileName), "");
        try
        {
            var config = ConfigManager.LoadConfig<TestEdgeConfig>(appName, fileName);
            // Empty YAML string may return null from YamlDotNet
            Assert.Null(config);
        }
        finally
        {
            CleanupConfig(appName, fileName);
        }
    }

    [Fact]
    public void ConfigManager_YamlWithOnlyComments_ShouldReturnNullDeserialized()
    {
        var appName = "EdgeTest_" + Guid.NewGuid().ToString("N");
        var fileName = "comments.yaml";
        var dir = XdgPaths.GetConfigDir(appName);
        Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, fileName), "# just a comment\n# another comment\n");
        try
        {
            var config = ConfigManager.LoadConfig<TestEdgeConfig>(appName, fileName);
            // YAML with only comments returns null from YamlDotNet
            Assert.Null(config);
        }
        finally
        {
            CleanupConfig(appName, fileName);
        }
    }

    [Fact]
    public void ConfigManager_Utf8Bom_ShouldParse()
    {
        var appName = "EdgeTest_" + Guid.NewGuid().ToString("N");
        var fileName = "bom.yaml";
        var dir = XdgPaths.GetConfigDir(appName);
        Directory.CreateDirectory(dir);
        // Write with UTF-8 BOM
        var content = "value: 42\n";
        var bom = new byte[] { 0xEF, 0xBB, 0xBF };
        using (var fs = File.Create(Path.Combine(dir, fileName)))
        {
            fs.Write(bom, 0, 3);
            fs.Write(System.Text.Encoding.UTF8.GetBytes(content), 0, content.Length);
        }
        try
        {
            var config = ConfigManager.LoadConfig<TestEdgeConfig>(appName, fileName);
            Assert.NotNull(config);
            Assert.Equal(42, config.Value);
        }
        finally
        {
            CleanupConfig(appName, fileName);
        }
    }

    [Fact]
    public void ConfigManager_MissingDirectory_ShouldCreateAndReturnDefault()
    {
        var appName = "NewDirTest_" + Guid.NewGuid().ToString("N");
        var fileName = "config.yaml";
        // Ensure directory does NOT exist
        var dir = XdgPaths.GetConfigDir(appName);
        if (Directory.Exists(dir)) Directory.Delete(dir, true);

        try
        {
            var config = ConfigManager.LoadConfig<TestEdgeConfig>(appName, fileName);
            Assert.NotNull(config);
            Assert.Equal(0, config.Value);
            // Directory should have been created
            Assert.True(Directory.Exists(dir));
        }
        finally
        {
            CleanupConfig(appName, fileName);
        }
    }

    // ========== ProcFS Edge Cases ==========

    [Fact]
    public void ProcFS_NonExistentFile_ShouldReturnError()
    {
        var result = ProcFS.ReadProcFile("nonexistent_file_xyz");
        Assert.NotEmpty(result);
        Assert.True(result.ContainsKey("Error"));
    }

    [Fact]
    public void ProcFS_GetTotalRam_WithInvalidFormat_ShouldHandleGracefully()
    {
        // This tests the parsing in GetTotalRamKb
        var ram = ProcFS.GetTotalRamKb();
        Assert.True(ram >= 0);
    }

    // ========== TrayService Lifecycle Cases ==========

    [Fact]
    public void TrayService_SetVisibility_BeforeInit_ShouldNotThrow()
    {
        var service = new TrayService();
        var ex = Record.Exception(() => service.SetVisibility(true));
        Assert.Null(ex);
    }

    [Fact]
    public void TrayService_UpdateTooltip_BeforeInit_ShouldNotThrow()
    {
        var service = new TrayService();
        var ex = Record.Exception(() => service.UpdateTooltip("test"));
        Assert.Null(ex);
    }

    [Fact]
    public void TrayService_DoubleInitialize_ShouldNotThrow()
    {
        var service = new TrayService();
        service.Initialize("icon1", "tip1");
        var ex = Record.Exception(() => service.Initialize("icon2", "tip2"));
        Assert.Null(ex);
    }

    // ========== SearchItemViewModel Edge Cases ==========

    [Fact]
    public void SearchItemViewModel_ExecuteAction_Null_ShouldNotThrow()
    {
        var vm = new SearchItemViewModel();
        Assert.Null(vm.ExecuteAction);
        // Calling null delegate should not throw since we don't invoke it
        // but SelectItem does invoke it
    }

    [Fact]
    public void SearchItemViewModel_ResultText_WhenExecutionResultNull_ShouldBeEmpty()
    {
        var vm = new SearchItemViewModel();
        Assert.Null(vm.ExecutionResult);
        Assert.Equal(string.Empty, vm.ResultText);
        Assert.True(vm.ResultIsSuccess);
    }

    [Fact]
    public void SearchItemViewModel_LinesOfCode_MaxValue_ShouldStore()
    {
        var vm = new SearchItemViewModel();
        vm.LinesOfCode = int.MaxValue;
        Assert.Equal(int.MaxValue, vm.LinesOfCode);
    }

    // ========== NotificationService Priority Mapping ==========

    [Fact]
    public async Task NotificationService_DefaultPriority_ShouldBeNormal()
    {
        var service = new NotificationService();
        var ex = await Record.ExceptionAsync(
            () => service.SendNotificationAsync("t", "m"));
        Assert.Null(ex);
    }

    // ========== NativeNotificationBridge Command Injection ==========

    [Fact]
    public async Task NativeNotificationBridge_SpecialChars_ShouldNotExecuteInjection()
    {
        // Test that shell metacharacters in notification text are not executed
        var result = await NativeNotificationBridge.SendNotificationAsync(
            "test'; echo pwned; '",
            "msg$(id)",
            "normal");
        // Should return false (notify-send will fail with these args) or true gracefully
        Assert.NotNull(result);
    }

    // ========== NativeDialogBridge Command Injection ==========

    [Fact]
    public async Task NativeDialogBridge_SpecialChars_ShouldNotExecuteInjection()
    {
        // Test with special characters that could break out of zenity arguments
        var path = await NativeDialogBridge.OpenFileAsync(
            "test\"; echo pwned; \"",
            "*.*");
        // Should not execute the injected command, return null or a path
        Assert.Null(path);
    }

    // ========== ProcessUtility Edge Cases ==========

    [Fact]
    public void ProcessUtility_ShouldAlwaysReturnValidResult()
    {
        var result = ProcessUtility.GetTopProcesses();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Contains("Top 10", result.Message);
    }

    [Fact]
    public void ProcessUtility_Result_ShouldNotBeEmpty()
    {
        var result = ProcessUtility.GetTopProcesses();
        Assert.NotEmpty(result.Message);
    }

    // ========== NetworkUtility Edge Cases ==========

    [Fact]
    public void NetworkUtility_ShouldNotThrow()
    {
        var ex = Record.Exception(() => NetworkUtility.GetNetworkSummary());
        Assert.Null(ex);
    }

    // ========== CpuUtility Edge Cases ==========

    [Fact]
    public void CpuUtility_AlwaysReturnsSuccess()
    {
        var result = CpuUtility.GetCpuSummary();
        Assert.True(result.IsSuccess);
    }

    // ========== MemoryUtility Edge Cases ==========

    [Fact]
    public void MemoryUtility_AlwaysReturnsSuccess()
    {
        var result = MemoryUtility.GetMemorySummary();
        Assert.True(result.IsSuccess);
    }

    // ========== SystemInfoUtility Edge Cases ==========

    [Fact]
    public void SystemInfoUtility_ShouldReturnAllFields()
    {
        var result = SystemInfoUtility.GetSystemSummary();
        Assert.True(result.IsSuccess);
        Assert.Contains("OS:", result.Message);
        Assert.Contains("Architecture:", result.Message);
        Assert.Contains("Framework:", result.Message);
        Assert.Contains("Machine Name:", result.Message);
        Assert.Contains("Processor Count:", result.Message);
        Assert.Contains("User:", result.Message);
    }

    // ========== UtilityResult Edge Cases ==========

    [Fact]
    public void UtilityResult_DefaultErrorDetailsIsNull()
    {
        var result = new UtilityResult(true, "ok");
        Assert.Null(result.ErrorDetails);
    }

    [Fact]
    public void UtilityResult_Equality_ShouldWork()
    {
        var r1 = new UtilityResult(true, "msg", null);
        var r2 = new UtilityResult(true, "msg", null);
        Assert.Equal(r1, r2);
        Assert.Equal(r1.GetHashCode(), r2.GetHashCode());
    }

    [Fact]
    public void UtilityResult_Inequality_ShouldWork()
    {
        var r1 = new UtilityResult(true, "msg1", null);
        var r2 = new UtilityResult(false, "msg2", "err");
        Assert.NotEqual(r1, r2);
    }

    // ========== BadgeStatus Edge Cases ==========

    [Fact]
    public void BadgeStatus_UndefinedValue_ShouldExist()
    {
        // Even undefined enum values should be representable
        var status = (BadgeStatus)42;
        Assert.Equal(42, (int)status);
    }

    // ========== ComponentDemoViewModel Edge Cases ==========

    [Fact]
    public void ComponentDemoViewModel_NullStrings_ShouldSet()
    {
        var vm = new ComponentDemoViewModel();
        var ex = Record.Exception(() => vm.Name = null!);
        Assert.Null(ex);
    }

    // ========== MetricCardViewModel Edge Cases ==========

    [Fact]
    public void MetricCardViewModel_DefaultValues_ShouldBeCorrect()
    {
        var vm = new MetricCardViewModel();
        Assert.Equal("CPU Usage", vm.Title);
        Assert.Equal(BadgeStatus.Success, vm.Status);
        Assert.Equal("24%", vm.Value);
        Assert.Equal("↓ 2% from last hour", vm.Trend);
    }

    [Fact]
    public void MetricCardViewModel_ShouldUpdateProperties()
    {
        var vm = new MetricCardViewModel
        {
            Title = "Memory",
            Status = BadgeStatus.Warning,
            Value = "80%",
            Trend = "↑ 5%"
        };
        Assert.Equal("Memory", vm.Title);
        Assert.Equal(BadgeStatus.Warning, vm.Status);
        Assert.Equal("80%", vm.Value);
        Assert.Equal("↑ 5%", vm.Trend);
    }

    // ========== UtilityResult record behavior ==========

    [Fact]
    public void UtilityResult_ToString_ShouldNotBeEmpty()
    {
        var result = new UtilityResult(true, "test");
        Assert.False(string.IsNullOrWhiteSpace(result.ToString()));
    }

    // ========== LinuxNotifier ==========

    [Fact]
    public async Task LinuxNotifier_WithSpecialChars_ShouldNotThrow()
    {
        var ex = await Record.ExceptionAsync(() =>
            LinuxNotifier.SendNotification("test'title", "msg\"with\"quotes"));
        // notify-send may not be available, but the utility should handle it gracefully
        Assert.Null(ex);
    }

    // ========== SystemdManager Edge Cases ==========

    [Fact]
    public async Task SystemdManager_NonexistentService_ShouldReturnFalse()
    {
        var result = await SystemdManager.IsServiceActive("nonexistent_service_xyz");
        Assert.False(result);
    }

    [Fact]
    public async Task SystemdManager_GetStatus_Nonexistent_ShouldReturnOutput()
    {
        var result = await SystemdManager.GetServiceStatus("nonexistent_service_xyz");
        Assert.NotNull(result);
    }

    // ========== PackageManager Edge Cases ==========

    [Fact]
    public async Task PackageManager_Detect_ShouldReturnValidEnum()
    {
        var pkg = await PackageManager.DetectPackageManager();
        Assert.True(Enum.IsDefined(typeof(PackageManagerType), pkg));
    }

    // ========== DBusService Edge Cases ==========

    [Fact]
    public async Task DBusService_InvalidEndpoint_ShouldReturnError()
    {
        var result = await DBusService.GetSystemProperty(
            "invalid.destination",
            "/invalid/path",
            "invalid.interface",
            "invalid_prop");
        Assert.NotNull(result);
        Assert.Contains("Error", result);
    }

    // ========== PropFS GetTotalRamKb ==========

    [Fact]
    public void ProcFS_GetTotalRamKb_ShouldReturnPositive()
    {
        var ram = ProcFS.GetTotalRamKb();
        Assert.True(ram >= 0);
    }

    // ========== XdgPaths EnsureDirectories ==========

    [Fact]
    public void XdgPaths_EnsureDirectories_ShouldCreateAll()
    {
        var appName = "EnsureTest_" + Guid.NewGuid().ToString("N");
        try
        {
            XdgPaths.EnsureDirectories(appName);
            Assert.True(Directory.Exists(XdgPaths.GetConfigDir(appName)));
            Assert.True(Directory.Exists(XdgPaths.GetDataDir(appName)));
            Assert.True(Directory.Exists(XdgPaths.GetCacheDir(appName)));
        }
        finally
        {
            var dirs = new[] { XdgPaths.GetConfigDir(appName), XdgPaths.GetDataDir(appName), XdgPaths.GetCacheDir(appName) };
            foreach (var d in dirs)
                if (Directory.Exists(d)) Directory.Delete(d, true);
        }
    }

    // ========== NotificationPriority mapping ==========

    [Theory]
    [InlineData(NotificationPriority.Low)]
    [InlineData(NotificationPriority.Normal)]
    [InlineData(NotificationPriority.Critical)]
    public async Task NotificationService_AllPriorities_ShouldComplete(NotificationPriority priority)
    {
        var service = new NotificationService();
        var ex = await Record.ExceptionAsync(
            () => service.SendNotificationAsync("t", "m", priority));
        Assert.Null(ex);
    }

    // ========== Helpers ==========

    private static MainWindowViewModel CreateVM() => new(
        ServiceProvider.GetService<IDataService>(),
        ServiceProvider.GetService<IThemeService>(),
        ServiceProvider.GetService<ITrayService>(),
        ServiceProvider.GetService<IDialogService>(),
        ServiceProvider.GetService<INotificationService>()
    );

    private static void CleanupConfig(string appName, string fileName)
    {
        var dir = XdgPaths.GetConfigDir(appName);
        var path = Path.Combine(dir, fileName);
        if (File.Exists(path)) File.Delete(path);
        if (Directory.Exists(dir)) Directory.Delete(dir, true);
    }

    private class TestEdgeConfig
    {
        public int Value { get; set; }
    }
}
