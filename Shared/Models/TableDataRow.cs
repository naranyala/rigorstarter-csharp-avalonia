using System;

namespace RigorStarter.Shared.Models;

public class TableDataRow
{
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
