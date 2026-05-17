using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RigorStarter.Utilities;

public static class ConfigManager
{
    private static readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    private static readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public static T LoadConfig<T>(string appName, string fileName)
        where T : new()
    {
        var path = Path.Combine(XdgPaths.GetConfigDir(appName), fileName);
        if (!File.Exists(path))
        {
            var defaultConfig = new T();
            SaveConfig(appName, fileName, defaultConfig);
            return defaultConfig;
        }

        try
        {
            var yaml = File.ReadAllText(path);
            return _deserializer.Deserialize<T>(yaml);
        }
        catch
        {
            return new T();
        }
    }

    public static void SaveConfig<T>(string appName, string fileName, T config)
    {
        var dir = XdgPaths.GetConfigDir(appName);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var path = Path.Combine(dir, fileName);
        var yaml = _serializer.Serialize(config);
        File.WriteAllText(path, yaml);
    }
}
