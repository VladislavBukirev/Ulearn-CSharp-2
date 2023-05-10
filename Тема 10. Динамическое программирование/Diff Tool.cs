using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);
    }

    private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
    {
        var opt = new int[first.Count + 1, second.Count + 1];
        for (var i = 0; i <= first.Count; i++)
            opt[i, 0] = 0;
        for (var i = 0; i <= second.Count; i++)
            opt[0, i] = 0;
        for(var i = 1; i <= first.Count; ++i)
        for (var j = 1; j < second.Count; ++j)
        {
            if (first[i - 1] == second[j - 1])
                opt[i, j] = 1 + opt[i - 1, j - 1];
            else
                opt[i, j] = Math.Max(opt[i - 1, j], opt[i, j - 1]);
        }
        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
    {
        var n = first.Count;
        var m = second.Count;
        if (n == 0 || m == 0)
            return new List<string>();
        if (first[n - 1] == second[m - 1])
        {
            var result = RestoreAnswer(opt, first.GetRange(0, n - 1), second.GetRange(0, m - 1));
            result.Add(first[n - 1]);
            return result;
        }
        return opt[n, m - 1] > opt[n - 1, m] 
            ? RestoreAnswer(opt, first, second.GetRange(0, m - 1)) 
            : RestoreAnswer(opt, first.GetRange(0, n - 1), second);
    }
}