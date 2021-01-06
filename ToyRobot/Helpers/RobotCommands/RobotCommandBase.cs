using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public interface IRobotCommand<TRobot> where TRobot : IRobot
    {
        public TRobot Execute(TRobot robot, Table table);
    }
    public abstract class RobotCommandBase<TRobot> : IRobotCommand<TRobot> where TRobot : IRobot
    {
        protected virtual bool ValidateCommand(TRobot robot, Table table, out string validationFailureMessage)
        {
            validationFailureMessage = null;
            if (robot.IsPlaced) return true;

            validationFailureMessage = $"{typeof(TRobot).Name} must be placed before trying to execute a command";
            return false;
        }

        public TRobot Execute(TRobot robot, Table table)
        {
            if (!ValidateCommand(robot, table, out var validationFailureMessage))
            {
                throw new RobotCommandException(validationFailureMessage);
            }

            robot = ExecuteCommand(robot, table);

            return robot;
        }

        protected abstract TRobot ExecuteCommand(TRobot robot, Table table);
    }
}
