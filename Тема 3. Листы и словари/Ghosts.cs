using System;
using System.Linq.Expressions;
using System.Text;

namespace hashes;

public class GhostsTask : 
    IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
    IMagic
{
    private byte[] encodingBytes = {1,2,3};
    private Vector vector;
    private Segment segment;
    private Document document;
    private Cat cat;
    private Robot robot;

    public GhostsTask()
    {
        vector = new Vector(0, 0);
        segment = new Segment(vector, vector);
        document = new Document("title", Encoding.Unicode, encodingBytes);
        cat = new Cat("Vlad", "house", DateTime.Now);
        robot = new Robot("15032004");
    }

    public void DoMagic()
    {
        encodingBytes[0] = 4;
        vector.Add(new Vector(1, 1));
        cat.Rename("Lucifer");
        Robot.BatteryCapacity++;
    }

    Vector IFactory<Vector>.Create()
    {
        return vector;
    }

    Segment IFactory<Segment>.Create()
    {
        return segment;
    }
	
    Document IFactory<Document>.Create()
    {
        return document;
    }

    Cat IFactory<Cat>.Create()
    {
        return cat;
    }

    Robot IFactory<Robot>.Create()
    {
        return robot;
    }
}