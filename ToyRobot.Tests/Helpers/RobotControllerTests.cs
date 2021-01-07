using System;
using System.IO;
using FluentAssertions;
using Moq;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Exceptions;
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
            _robotCommand.Setup(r => r.ValidateAndExecute(It.IsAny<IRobot>(), It.IsAny<Table>())).Returns(robotAfterCommands);
            var result = _sut.ExecuteCommands(_robot.Object);

            result.Should().BeEquivalentTo(robotAfterCommands);
            _robotCommand.Verify(r => r.ValidateAndExecute(It.IsAny<IRobot>(), It.IsAny<Table>()), Times.Once);
        }

        [Fact]
        public void ExecuteCommands_WritesMessageWhenCommandFails()
        {
            _sut.AddCommand(_robotCommand.Object);
            _robotCommand.Setup(r => r.ValidateAndExecute(It.IsAny<IRobot>(), It.IsAny<Table>())).Throws(new RobotCommandException("we failed"));

            // method suggested at https://stackoverflow.com/questions/2139274/grabbing-the-output-sent-to-console-out-from-within-a-unit-test
            using var sw = new StringWriter();
            Console.SetOut(sw);

            var _ = _sut.ExecuteCommands(_robot.Object);
            var expected = $"we failed{Environment.NewLine}";
            sw.ToString().Should().Be(expected);
        }

        [Fact]
        public void ExecuteSingleCommand_ExecutesCommand()
        {
            var robotAfterCommand = new ToyRobot
            {
                Facing = CompassDirection.North,
                IsPlaced = true,
                Position = new RobotPosition { X = 100, Y = 50 }
            };

            _robotCommand.Setup(r => r.ValidateAndExecute(It.IsAny<IRobot>(), It.IsAny<Table>())).Returns(robotAfterCommand);
            var result = _sut.ExecuteSingleCommand(_robotCommand.Object, _robot.Object);

            result.Should().BeEquivalentTo(robotAfterCommand);
            _robotCommand.Verify(r => r.ValidateAndExecute(It.IsAny<IRobot>(), It.IsAny<Table>()), Times.Once);
        }
    }
}
