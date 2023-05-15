using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree;

public static class DiskTreeTask
{
    public static List<string> Solve(List<string> directories)
    {
        var root = new TreeNode { DirectoryName = "", IsRoot = true };
        foreach (var directory in directories)
            root.Add(directory.Split('\\'), 0);
        return root.GetDirectories(0).ToList();
    }
}

public class TreeNode
{
    public string DirectoryName;
    public readonly List<TreeNode> Children = new List<TreeNode>();
    public bool IsRoot = false;

    public void Add(string[] directories, int index)
    {
        if (index == directories.Length)
            return;
        var folder = Children.Find(x => x.DirectoryName == directories[index]);
        if (folder == null)
        {
            folder = new TreeNode { DirectoryName = directories[index] };
            Children.Add(folder);
        }

        folder.Add(directories, index + 1);
    }

    public IEnumerable<string> GetDirectories(int offset=0)
    {
        if(!IsRoot)
        {
            yield return new string(' ', offset) + DirectoryName;
            offset++;
        }

        var subFolders = Children
            .OrderBy(dir => dir.DirectoryName, StringComparer.Ordinal)
            .SelectMany(dir => dir.GetDirectories(offset));
        foreach (var folder in subFolders)
            yield return folder;
    }
}