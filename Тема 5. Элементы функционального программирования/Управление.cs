using System;

namespace func_rocket;

public class ControlTask
{
    public static Turn ControlRocket(Rocket rocket, Vector target)
    {
        var distToTarget = new Vector(target.X - rocket.Location.X, target.Y - rocket.Location.Y);
        var rocketDirection = rocket.Velocity.Angle * 2 / 3 + rocket.Direction / 3;
        var difAngle = distToTarget.Angle - rocketDirection;
        return difAngle > 0 ? Turn.Right : Turn.Left;
    }
}