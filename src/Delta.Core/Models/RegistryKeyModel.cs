namespace Delta.Core.Models;
public class RegistryKeyModel : Dictionary<string, RegistryKeyModel>
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
