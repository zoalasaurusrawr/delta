using Delta.Core.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Core.Analysis;

internal class RegistryAnalyzer
{

    internal RegistryModel Analyze()
    {
        var results = new RegistryModel();
        var subkeyNames = Registry.CurrentUser.GetSubKeyNames() ?? Enumerable.Empty<string>();
        results.CurrentUser = IterateRegistry(Registry.CurrentUser);
        results.Configuration = IterateRegistry(Registry.CurrentConfig);
        results.LocalMachine = IterateRegistry(Registry.LocalMachine);
        results.Users = IterateRegistry(Registry.Users);
        return results;
    }

    public RegistryKeyModel IterateRegistry(RegistryKey root, RegistryKey? parent = null)
    {
        var results = new RegistryKeyModel();

        try
        {

            // Process keys
            foreach (string keyName in GetSubKeyNamesSafe(root))
            {
                using (RegistryKey subKey = root.OpenSubKey(keyName))
                {
                    // Your code to process the key here

                    // Recursive call for the subkey
                    var subkeyResults = IterateRegistry(subKey, root);
                    results.Add($"{subKey.Name}", subkeyResults);
                }
            }

            // Process values
            foreach (string valueName in GetSubKeyValueNamesSafe(root))
            {
                var name = $"{parent?.Name ?? string.Empty}/{valueName}";
                // Your code to process the value here
                var value = root.GetValue(valueName)?.ToString() ?? string.Empty;
                results.Add(name, new RegistryKeyModel { Name = valueName, Value = value });
            }

            return results;
        }
        catch (Exception ex)
        {
            return results;
        }
        
    }

    private string[] GetSubKeyNamesSafe(RegistryKey key)
    {
        try
        {
            return key.GetSubKeyNames();
        }
        catch (Exception)
        {
            return Enumerable.Empty<string>().ToArray();
        }
    }

    private string[] GetSubKeyValueNamesSafe(RegistryKey key)
    {
        try
        {
            return key.GetValueNames();
        }
        catch (Exception)
        {
            return Enumerable.Empty<string>().ToArray();
        }
    }
}
