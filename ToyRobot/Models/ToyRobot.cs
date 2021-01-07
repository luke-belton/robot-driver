using System;
using ToyRobotSimulator.Constants;

namespace ToyRobotSimulator.Models
{
    public interface IRobotReporter
    {
        /// <summary>
        /// Report the robot's current position
        /// </summary>
        public void ReportPosition();
    }

    public interface IRobotLocator
    {
        /// <summary>
        /// Indicate whether the robot is placed
        /// </summary>
        public bool IsPlaced { get; set; }

        /// <summary>
        /// Current position of the robot
        /// </summary>
        public RobotPosition Position { get; set; }

        /// <summary>
        /// Direction on compass the robot is currently facing
        /// </summary>
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
        /// <summary>
        /// X coordinate of robot
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate of robot
        /// </summary>
        public int Y { get; set; }
    }
}
