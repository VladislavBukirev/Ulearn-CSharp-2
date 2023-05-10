using System;
using System.Collections.Generic;
using System.Linq;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        var result = new List<ComparisonResult>();
        for (var i = 0; i < documents.Count; i++)
        for (var j = i + 1; j < documents.Count; j++)
            result.Add(CompareDocuments(documents[i], documents[j]));
        return result;
    }

    ComparisonResult CompareDocuments(DocumentTokens first, DocumentTokens second)
    {
        var prev = new double[second.Count + 1];
        var curr = new double[second.Count + 1];
        for (var i = 0; i <= second.Count; ++i)
            prev[i] = i;
        for (var i = 1; i <= first.Count; i++)
        {
            curr[0] = i;
            for (var j = 1; j <= second.Count; j++)
            {
                if (first[i - 1] == second[j - 1])
                    curr[j] = prev[j - 1];
                else
                {
                    var tokenDistance = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
                    curr[j] = new List<double>
                        { 1 + curr[j - 1], tokenDistance + prev[j - 1], 1 + prev[j] }.Min();
                }
            }
            Array.Copy(curr, prev, second.Count + 1);
        }
        var result = prev[second.Count];
        return new ComparisonResult(first, second, result);
    }
}