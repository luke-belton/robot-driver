using System;
using ToyRobotSimulator.Constants;

namespace ToyRobotSimulator.Models
{
    public interface IRobotReporter
    {
        public void ReportPosition();
    }

    public interface IRobotLocator
    {
        public bool IsPlaced { get; set; }

        public RobotPosition Position { get; set; }

        public CompassDirection Facing { get; set; }
    }

    public interface IRobot : IRobotLocator, IRobotReporter { }

    public class ToyRobot : IRobot
    {
        public bool IsPlaced { get; set; } = false;

        public RobotPosition Position { get; set; }

        public CompassDirection Facing { get; set; }

        public void ReportPosition() => Console.WriteLine(ToString());

        public override string ToString()
        {
            return $"{Position.X},{Position.Y},{Facing.ToString().ToUpper()}";
        }
    }

    public class RobotPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
