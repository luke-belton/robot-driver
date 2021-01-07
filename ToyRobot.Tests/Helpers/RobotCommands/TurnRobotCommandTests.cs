using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Models;
using Xunit;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class TurnRobotCommandTests
    {
        private readonly Mock<IRobot> _robot;
        private readonly Table _table;

        public TurnRobotCommandTests()
        {
            _robot = new Mock<IRobot>();
            _table = new Table() { XDimension = 5, YDimension = 5 };
        }

        [Fact]
        public void Execute_ThrowsException_WhenRobotNotPlaced()
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(false);
            var command = new TurnRobotCommand<IRobot>(TurnDirection.Left);

            Assert.Throws<RobotCommandException>(() => command.ValidateAndExecute(_robot.Object, _table));
        }

        [InlineData(CompassDirection.North, CompassDirection.West, TurnDirection.Left)]
        [InlineData(CompassDirection.West, CompassDirection.South, TurnDirection.Left)]
        [InlineData(CompassDirection.South, CompassDirection.East, TurnDirection.Left)]
        [InlineData(CompassDirection.East, CompassDirection.North, TurnDirection.Left)]
        [InlineData(CompassDirection.North, CompassDirection.East, TurnDirection.Right)]
        [InlineData(CompassDirection.West, CompassDirection.North, TurnDirection.Right)]
        [InlineData(CompassDirection.South, CompassDirection.West, TurnDirection.Right)]
        [InlineData(CompassDirection.East, CompassDirection.South, TurnDirection.Right)]
        [Theory]
        public void Execute_PerformsCorrectTurns(CompassDirection currentFacing, CompassDirection expectedFacing, TurnDirection turnDirection)
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(true);
            var command = new TurnRobotCommand<IRobot>(turnDirection);

            _robot.SetupGet(r => r.Facing).Returns(currentFacing);
            command.ValidateAndExecute(_robot.Object, _table);
            _robot.VerifySet(r => r.Facing = expectedFacing);
        }
    }
}
