using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket;

public class LevelsTask
{
    private static readonly Physics standardPhysics = new();

    private static readonly Rocket rocketLocation =
        new(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

    private static readonly Vector baseTarget = new Vector(600, 200);

    public static IEnumerable<Level> CreateLevels()
    {
        yield return CreateLevel("Zero", (size, location) => Vector.Zero);
        yield return CreateLevel("Heavy", (size, location) => new Vector(0, 0.9));
        yield return CreateLevel("Up", target: new Vector(700, 500),
            gravity: (size, location) => new Vector(0, -300 / (size.Y - location.Y + 300)));
        yield return CreateLevel("WhiteHole", gravity: (size, location) => CalculateWhiteHole(location));
        yield return CreateLevel("BlackHole", gravity: (size, location) => CalculateBlackHole(location));
        yield return CreateLevel("BlackAndWhite",
            gravity: (size, location) => CalculateBlackAndWhite(location));
    }

    private static Level CreateLevel
        (string name, Gravity gravity, Rocket rocket = null!, Vector target = null!, Physics physics = null!)
    {
        rocket ??= rocketLocation;
        target ??= baseTarget;
        physics ??= standardPhysics;
        return new Level(name, rocket, target, gravity, physics);
    }

    private static Vector CalculateWhiteHole(Vector v)
    {
        var distToTarg = new Vector(v.X - 600, v.Y - 200);
        return distToTarg.Normalize() * 140 * distToTarg.Length / (distToTarg.Length * distToTarg.Length + 1);
    }

    private static Vector CalculateBlackHole(Vector v)
    {
        var anomalyVect = new Vector(200 + 600, 500 + 200) / 2;
        var distToTarg = new Vector(anomalyVect.X - v.X, anomalyVect.Y - v.Y);
        return distToTarg.Normalize() * 300 * distToTarg.Length / (distToTarg.Length * distToTarg.Length + 1);
    }

    private static Vector CalculateBlackAndWhite(Vector v)
    {
        var distToTarg1 = new Vector(v.X - 600, v.Y - 200);
        var d1 = distToTarg1.Normalize() * 140 * distToTarg1.Length /
                 (distToTarg1.Length * distToTarg1.Length + 1);
        var anomalyVect = new Vector(200 + 600, 500 + 200) / 2;
        var distToTarg2 = new Vector(anomalyVect.X - v.X, anomalyVect.Y - v.Y);
        var d2 = distToTarg2.Normalize() * 300 * distToTarg2.Length /
                 (distToTarg2.Length * distToTarg2.Length + 1);
        return (d1 + d2) / 2;
    }
}