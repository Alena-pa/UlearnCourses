using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        var tasks = new List<Task<(Turn Turn, double Score)>>();
        for (int i = 0; i < threadsCount; i++)
        {
            var localRandom = new Random(random.Next());
            tasks.Add(Task.Run(() => SearchBestMove(rocket, localRandom, iterationsCount / threadsCount)));
        }

        var results = Task.WhenAll(tasks).GetAwaiter().GetResult();

        var best = results.OrderByDescending(r => r.Score).First();

        return rocket.Move(best.Turn, level);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket) => new();
}