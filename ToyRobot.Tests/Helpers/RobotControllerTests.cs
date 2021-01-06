using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Helpers.RobotCommands;
using ToyRobotSimulator.Models;
using Xunit;

namespace ToyRobotSimulator.Helpers
{
    public class RobotControllerTests
    {
        private readonly Mock<IRobot> _robot;
        private readonly Mock<IRobotCommand<IRobot>> _robotCommand;
        private readonly RobotController<IRobot> _sut;

        public RobotControllerTests()
        {
            _robot = new Mock<IRobot>();

            var table = new Table() {XDimension = 5, YDimension = 5};
            _sut = new RobotController<IRobot>(table);

            _robotCommand = new Mock<IRobotCommand<IRobot>>();
        }

        [Fact]
        public void AddCommand_AddsCommandToList()
        {
            _sut.Commands.Count.Should().Be(0);
            _sut.AddCommand(_robotCommand.Object);
            _sut.Commands.Count.Should().Be(1);
        }

        [Fact]
        public void ExecuteCommands_CallsExecute()
        {
            var robotAfterCommands = new ToyRobot
            {
                Facing = CompassDirection.North,
                IsPlaced = true,
                Position = new RobotPosition {X = 100, Y = 50}
            };

            _sut.AddCommand(_robotCommand.Object);
            _robotCommand.Setup(r => r.Execute(It.IsAny<IRobot>(), It.IsAny<Table>())).Returns(robotAfterCommands);
            var result = _sut.ExecuteCommands(_robot.Object);

            result.Should().BeEquivalentTo(robotAfterCommands);
            _robotCommand.Verify(r => r.Execute(It.IsAny<IRobot>(), It.IsAny<Table>()), Times.Once);
        }
    }
}
