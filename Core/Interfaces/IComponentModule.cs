using System;

namespace RigorStarter.Core.Interfaces;

public enum ComponentCategory
{
    Pinned,
    InDevelopment,
    Archives,
}

public interface IComponentModule
{
    string Name { get; }
    string Description { get; }
    ComponentCategory Category { get; }
    Type ViewType { get; }
    bool IsMockup { get; }
}
