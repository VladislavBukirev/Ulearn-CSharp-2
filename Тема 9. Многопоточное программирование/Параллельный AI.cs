using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        var allMoves = CreateTasks(rocket);
        Task.WaitAll(allMoves.ToArray());
        var bestMove = allMoves.Select(task => task.Result).Max();
        return rocket.Move(bestMove.Turn, level);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        var tasks = new List<Task<(Turn Turn, double Score)>>();
        for (var i = 0; i < threadsCount; i++)
        {
            tasks.Add(Task.Run(() => SearchBestMove(rocket, new Random(random.Next()),
                iterationsCount / threadsCount)));
        }

        return tasks;
    }
}