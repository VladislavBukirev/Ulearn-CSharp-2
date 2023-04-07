using System;
using System.Linq;

namespace func_rocket;

public class ForcesTask
{
    public static RocketForce GetThrustForce(double forceValue)
    {
        return r => forceValue * new Vector(1, 0).Rotate(r.Direction);
    }

    public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize)
    {
        return r => gravity(spaceSize, r.Location);
    }

    public static RocketForce Sum(params RocketForce[] forces)
    {
        return rocket => forces.Aggregate(Vector.Zero, (current, force) => current + force(rocket));
    }
}