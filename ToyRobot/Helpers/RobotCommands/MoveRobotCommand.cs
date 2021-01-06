using System;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class MoveRobotCommand<TRobot> : RobotCommandBase<TRobot> where TRobot : IRobot
    {
        protected override bool ValidateCommand(TRobot robot, Table table, out string validationFailureMessage)
        {
            validationFailureMessage = null;
            if (!robot.IsPlaced)
            {
                validationFailureMessage = $"{typeof(TRobot).Name} must be placed before trying to move the Robot!";
                return false;
            }

            var prospectivePosition = GetNextPosition(robot);

            if (!table.ContainsCoordinate(prospectivePosition.X, prospectivePosition.Y))
            {
                validationFailureMessage = $"Moving the {typeof(TRobot).Name} now would take it off the table!";
                return false;
            }

            return true;
        }

        protected override TRobot ExecuteCommand(TRobot robot, Table table)
        {
            robot.Position = GetNextPosition(robot);
            return robot;
        }

        private static RobotPosition GetNextPosition(TRobot robot)
        {
            return robot.Facing switch
            {
                CompassDirection.North => new RobotPosition {X = robot.Position.X, Y = robot.Position.Y + 1},
                CompassDirection.East => new RobotPosition {X = robot.Position.X + 1, Y = robot.Position.Y},
                CompassDirection.South => new RobotPosition {X = robot.Position.X, Y = robot.Position.Y - 1},
                CompassDirection.West => new RobotPosition {X = robot.Position.X - 1, Y = robot.Position.Y},
                _ => throw new ArgumentException($"Cannot move {typeof(TRobot).Name} in direction {robot.Facing}")
            };
        }
    }
}
