using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;
using RigorStarter.ViewModels;

namespace RigorStarter.Core.Services;

public class ComponentRegistry
{
    private readonly List<IComponentModule> _modules = new();

    public IReadOnlyList<IComponentModule> Modules => _modules.AsReadOnly();

    public void Register(IComponentModule module)
    {
        if (!_modules.Contains(module))
        {
            _modules.Add(module);
        }
    }

    public void DiscoverModules()
    {
        var moduleTypes = new List<Type>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                moduleTypes.AddRange(
                    assembly
                        .GetTypes()
                        .Where(p =>
                            typeof(IComponentModule).IsAssignableFrom(p)
                            && !p.IsInterface
                            && !p.IsAbstract
                        )
                );
            }
            catch (ReflectionTypeLoadException ex)
            {
                moduleTypes.AddRange(
                    ex.Types.Where(t =>
                        t != null
                        && typeof(IComponentModule).IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract
                    )
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to scan assembly {assembly.FullName}: {ex.Message}");
            }
        }

        foreach (var type in moduleTypes)
        {
            try
            {
                var instance = (IComponentModule)Activator.CreateInstance(type)!;
                Register(instance);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to instantiate module {type.Name}: {ex.Message}");
            }
        }
    }
}
