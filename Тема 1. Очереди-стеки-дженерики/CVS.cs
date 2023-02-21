using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    private List<Clone> clonesList = new();

    public string Execute(string query)
    {
        if (clonesList.Count == 0) clonesList.Add(new Clone());
        var splitedCommand = query.Split(' ');
        var command = splitedCommand[0];
        var cloneIndex = int.Parse(splitedCommand[1]) - 1;
        switch (command)
        {
            case "learn":
                clonesList[cloneIndex].Learn(splitedCommand[2]);
                break;
            case "rollback":
                clonesList[cloneIndex].Rollback();
                break;
            case "relearn":
                clonesList[cloneIndex].Relearn();
                break;
            case "clone":
                var clonedClone = new Clone(clonesList[cloneIndex]);
                clonesList.Add(clonedClone);
                break;
            case "check":
                return clonesList[cloneIndex].Check();
        }
        return null;
    }
}

public class Clone
{
    private ItemsStack<string> learnedPrograms;
    private ItemsStack<string> cancelledPrograms;

    public Clone()
    {
        learnedPrograms = new ItemsStack<string>();
        cancelledPrograms = new ItemsStack<string>();
    }

    public void Learn(string program)
    {
        learnedPrograms.Push(program);
    }

    public void Rollback()
    {
        cancelledPrograms.Push(learnedPrograms.Pop());
    }

    public void Relearn()
    {
        learnedPrograms.Push(cancelledPrograms.Pop());
    }

    public Clone(Clone clonedClone)
    {
        learnedPrograms = new ItemsStack<string>()
        {
            LastOperation = clonedClone.learnedPrograms.LastOperation,
            ProgramsCount = clonedClone.learnedPrograms.ProgramsCount
        };
        cancelledPrograms = new ItemsStack<string>()
        {
            LastOperation = clonedClone.cancelledPrograms.LastOperation,
            ProgramsCount = clonedClone.cancelledPrograms.ProgramsCount
        };
    }

    public string Check()
    {
        if (learnedPrograms.ProgramsCount == 0)
            return "basic";
        var temp = learnedPrograms.Pop();
        learnedPrograms.Push(temp);
        return temp;
    }
}

public class Items<T>
{
    public T Value;
    public Items<T> PreviousOperation;
}

public class ItemsStack<T>
{
    public Items<T> LastOperation;
    public int ProgramsCount;


    public void Push(T item)
    {
        var newItem = new Items<T> { Value = item, PreviousOperation = LastOperation };
        LastOperation = newItem;
        ProgramsCount++;
    }

    public T Pop()
    {
        var result = LastOperation.Value;
        LastOperation = LastOperation.PreviousOperation;
        ProgramsCount--;
        return result;
    }
}