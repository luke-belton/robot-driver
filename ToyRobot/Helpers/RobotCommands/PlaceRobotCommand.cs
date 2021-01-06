using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class PlaceRobotCommand<TRobot> : RobotCommandBase<TRobot> where TRobot : IRobot
    {
        private readonly int _x;
        private readonly int _y;
        private readonly CompassDirection _facing;
        public PlaceRobotCommand(int x, int y, CompassDirection facing)
        {
            _x = x;
            _y = y;
            _facing = facing;
        }

        protected override bool ValidateCommand(TRobot robot, Table table, out string validationFailureMessage)
        {
            validationFailureMessage = null;
            if (table.ContainsCoordinate(_x, _y)) return true;

            validationFailureMessage = $"Cannot place {typeof(TRobot).Name} outside {nameof(Table)}";
            return false;

        }

        protected override TRobot ExecuteCommand(TRobot robot, Table table)
        {
            robot.Facing = _facing;
            robot.Position = new RobotPosition { X = _x, Y = _y };

            return robot;
        }
    }
}
