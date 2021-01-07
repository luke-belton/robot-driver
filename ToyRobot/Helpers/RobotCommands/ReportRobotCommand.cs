using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class ReportRobotCommand<TRobot> : RobotCommandBase<TRobot> where TRobot : IRobot
    {
        /// <summary>
        /// Report position of the <see cref="IRobot"/> on a table
        /// </summary>
        public ReportRobotCommand() { }

        protected override TRobot ExecuteCommand(TRobot robot, Table table)
        {
            robot.ReportPosition();
            return robot;
        }
    }
}
