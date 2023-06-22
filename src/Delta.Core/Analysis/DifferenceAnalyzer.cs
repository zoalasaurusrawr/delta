using Delta.Core.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Delta.Core.Analysis;
internal class DifferenceAnalyzer
{
    public DifferenceModel Compare(RegistryModel left, RegistryModel right)
    {
        var leftNode = JsonNode.Parse(JsonSerializer.Serialize(left));
        var rightNode = JsonNode.Parse(JsonSerializer.Serialize(right));
        var results = new DifferenceModel();


        results.Differences.AddRange(GetDifferences(left.Users, right.Users));
        results.Differences.AddRange(GetDifferences(left.Users, right.CurrentUser));
        results.Differences.AddRange(GetDifferences(left.Users, right.LocalMachine));
        results.Differences.AddRange(GetDifferences(left.Users, right.Configuration));

        return results;
    }

    private IEnumerable<DifferenceEntry> GetDifferences(RegistryKeyModel left, RegistryKeyModel right)
    {
        foreach (var item in left)
        {
            if (!right.ContainsKey(item.Key))
            {
                yield return new DifferenceEntry(item.Key, item.Value, null, DifferenceType.Removed);
            }

            if (right.ContainsKey(item.Key))
            {
                var rightValue = right[item.Key];
                var different = item.Value != rightValue;

                if (different)
                    yield return new DifferenceEntry(item.Key, item.Value, rightValue, DifferenceType.Changed);
            }
        }

        foreach (var item in right)
        {
            if (!left.ContainsKey(item.Key))
            {
                yield return new DifferenceEntry(item.Key, item.Value, null, DifferenceType.Added);
            }
        }
    }
}
