using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class ReportRobotCommand<TRobot> : RobotCommandBase<TRobot> where TRobot : IRobot
    {
        protected override TRobot ExecuteCommand(TRobot robot, Table table)
        {
            robot.ReportPosition();
            return robot;
        }
    }
}
