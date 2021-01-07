using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public interface IRobotCommand<TRobot> where TRobot : IRobot
    {
        /// <summary>
        /// Validates command for given robot and if valid, executes it
        /// </summary>
        /// <param name="robot">A <see cref="IRobot"/> implementation</param>
        /// <param name="table">A <see cref="Table"/></param>
        /// <returns>The <see cref="IRobot"/> after completing the supplied command</returns>
        /// <exception cref="RobotCommandException">Thrown when validation of the command fails. Exception message will contain a description of the error.</exception>
        public TRobot ValidateAndExecute(TRobot robot, Table table);
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

        public TRobot ValidateAndExecute(TRobot robot, Table table)
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
