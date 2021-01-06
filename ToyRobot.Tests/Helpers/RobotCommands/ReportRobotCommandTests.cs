using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Models;
using Xunit;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class ReportRobotCommandTests
    {
        private readonly Mock<IRobot> _robot;
        private readonly Table _table;

        public ReportRobotCommandTests()
        {
            _robot = new Mock<IRobot>();
            _table = new Table() { XDimension = 5, YDimension = 5 };
        }

        [Fact]
        public void Execute_ThrowsException_WhenRobotNotPlaced()
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(false);
            var command = new ReportRobotCommand<IRobot>();

            Assert.Throws<RobotCommandException>(() => command.Execute(_robot.Object, _table));
        }

        [Fact]
        public void Execute_CallsReportPosition_WhenRobotIsPlaced()
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(true);
            var command = new ReportRobotCommand<IRobot>();

            command.Execute(_robot.Object, _table);
            _robot.Verify(r => r.ReportPosition(), Times.Once);
        }
    }
}
