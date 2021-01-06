using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Helpers.RobotCommands;
using ToyRobotSimulator.Models;
using Xunit;

namespace ToyRobotSimulator.Helpers
{
    public class CommandManagerTests
    {
        private readonly CommandManager<IRobot> _sut;
        public CommandManagerTests()
        {
            _sut = new CommandManager<IRobot>();
        }

        [Fact]
        public void GetCommandFromInput_ThrowsException_WhenInputNullOrEmpty()
        {
            Assert.Throws<RobotCommandException>(() => _sut.GetCommandFromInput(null));
            Assert.Throws<RobotCommandException>(() => _sut.GetCommandFromInput(""));
        }

        [Fact]
        public void GetCommandFromInput_ReturnsPlaceCommand_WhenInputIsPlace()
        {
            var command = _sut.GetCommandFromInput($"{Commands.Place} 4,5,NORTH");

            command.Should().BeOfType<PlaceRobotCommand<IRobot>>();
            ((PlaceRobotCommand<IRobot>)command).XPlacement.Should().Be(4);
            ((PlaceRobotCommand<IRobot>)command).YPlacement.Should().Be(5);
            ((PlaceRobotCommand<IRobot>)command).Facing.Should().Be(CompassDirection.North);
        }

        [Fact]
        public void GetCommandFromInput_ReturnsMoveCommand_WhenInputIsMove()
        {
            var command = _sut.GetCommandFromInput($"{Commands.Move}");

            command.Should().BeOfType<MoveRobotCommand<IRobot>>();
        }

        [InlineData(Commands.Left, TurnDirection.Left)]
        [InlineData(Commands.Right, TurnDirection.Right)]
        [Theory]
        public void GetCommandFromInput_ReturnsTurnCommand_WhenInputIsTurn(string inputCommand, TurnDirection direction)
        {
            var command = _sut.GetCommandFromInput(inputCommand);

            command.Should().BeOfType<TurnRobotCommand<IRobot>>();

            ((TurnRobotCommand<IRobot>)command).TurnDirection.Should().Be(direction);
        }

        [Fact]
        public void GetCommandFromInput_ReturnsReportCommand_WhenInputIsReport()
        {
            var command = _sut.GetCommandFromInput(Commands.Report);

            command.Should().BeOfType<ReportRobotCommand<IRobot>>();
        }

        [Fact]
        public void GetCommandFromInput_ThrowsException_WhenSingleCommandHasExtraInstructions()
        {
            Assert.Throws<RobotCommandException>(() =>_sut.GetCommandFromInput($"{Commands.Report} plus_some_extra_nonsense"));
        }

        [Fact]
        public void GetCommandFromInput_ThrowsException_WhenCommandNotRecognised()
        {
            Assert.Throws<RobotCommandException>(() => _sut.GetCommandFromInput($"this is a nonsense command!"));
        }

        [Fact]
        public void GetCommandFromInput_ThrowsException_WhenPlaceCommandDoesNotHaveValidPlacementInstructions()
        {
            Assert.Throws<RobotCommandException>(() => _sut.GetCommandFromInput($"{Commands.Place} not_valid!"));
        }

        [Fact]
        public void GetCommandFromInput_ThrowsException_WhenPlaceCommandDoesNotHaveValidCompassDirection()
        {
            Assert.Throws<RobotCommandException>(() => _sut.GetCommandFromInput($"{Commands.Place} 4,5,OTHER_COMPASS_DIRECTION"));
        }

        [Fact]
        public void GetCommandFromInput_ThrowsException_WhenPlaceCommandDoesNotHaveAnyPlacementInstructions()
        {
            Assert.Throws<RobotCommandException>(() => _sut.GetCommandFromInput(Commands.Place));
        }
    }
}
