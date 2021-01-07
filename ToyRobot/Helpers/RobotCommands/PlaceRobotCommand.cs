using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class PlaceRobotCommand<TRobot> : RobotCommandBase<TRobot> where TRobot : IRobot
    {
        public int XPlacement { get; }
        public int YPlacement { get; }
        public CompassDirection Facing { get; }

        /// <summary>
        /// Place an <see cref="IRobot"/> on a table. Robot will not be placed if position is outside table.
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="facing">Facing direction</param>
        public PlaceRobotCommand(int x, int y, CompassDirection facing)
        {
            XPlacement = x;
            YPlacement = y;
            Facing = facing;
        }

        protected override bool ValidateCommand(TRobot robot, Table table, out string validationFailureMessage)
        {
            validationFailureMessage = null;
            if (table.ContainsCoordinate(XPlacement, YPlacement)) return true;

            validationFailureMessage = $"Cannot place {typeof(TRobot).Name} outside {nameof(Table)}";
            return false;

        }

        protected override TRobot ExecuteCommand(TRobot robot, Table table)
        {
            robot.Facing = Facing;
            robot.Position = new RobotPosition { X = XPlacement, Y = YPlacement };
            robot.IsPlaced = true;

            return robot;
        }
    }
}
