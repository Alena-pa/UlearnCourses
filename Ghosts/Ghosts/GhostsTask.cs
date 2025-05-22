using System;
using System.Text;
using System.Reflection;

namespace hashes;

public class GhostsTask :
    IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
    IMagic
{
    private readonly Vector vector = new(1, 1);
    private readonly Segment segment;
    private readonly Cat cat = new("Murchik", "British-shorthair", DateTime.Today);
    private readonly Robot robot = new("1231", 10.0);
    private readonly Document document = new("LOVE", Encoding.UTF8, Encoding.UTF8.GetBytes("Maria"));

    public GhostsTask()
    {
        segment = new Segment(vector, new Vector(2, 4));
    }

    public void DoMagic()
    {
        Robot.BatteryCapacity = new Random().Next(0, 100);
        vector.Add(new Vector(-10, 1));
        cat.Rename("Leo");

        var contentField = typeof(Document)
            .GetField("content", BindingFlags.NonPublic | BindingFlags.Instance);
        contentField?.SetValue(document, Encoding.UTF8.GetBytes("12"));
    }

    Vector IFactory<Vector>.Create() => vector;
    Segment IFactory<Segment>.Create() => segment;
    Document IFactory<Document>.Create() => document;
    Cat IFactory<Cat>.Create() => cat;
    Robot IFactory<Robot>.Create() => robot;
}
