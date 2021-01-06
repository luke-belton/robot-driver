using System;
using System.Collections.Generic;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class TurnRobotCommand<TRobot> : RobotCommandBase<TRobot> where TRobot : IRobot
    {
        private readonly TurnDirection _turnDirection;

        private readonly List<CompassDirection> orderedCompassDirections = new List<CompassDirection>
        {
            CompassDirection.North,
            CompassDirection.East,
            CompassDirection.South,
            CompassDirection.West,
        };

        public TurnRobotCommand(TurnDirection turnDirection)
        {
            _turnDirection = turnDirection;
        }

        protected override TRobot ExecuteCommand(TRobot robot, Table table)
        {
            robot.Facing = GetNewDirection(_turnDirection, robot.Facing);
            return robot;
        }

        private CompassDirection GetNewDirection(TurnDirection turnDirection, CompassDirection facingDirection)
        {
            var currentIndex = orderedCompassDirections.IndexOf(facingDirection);
            return turnDirection switch
            {
                TurnDirection.Left => currentIndex.Equals(0)
                    ? orderedCompassDirections[^1]
                    : orderedCompassDirections[currentIndex - 1],
                TurnDirection.Right => currentIndex.Equals(orderedCompassDirections.Count - 1)
                    ? orderedCompassDirections[0]
                    : orderedCompassDirections[currentIndex + 1],
                _ => throw new ArgumentException($"{nameof(TurnDirection)} must either be Left or Right")
            };
        }
    }
}
