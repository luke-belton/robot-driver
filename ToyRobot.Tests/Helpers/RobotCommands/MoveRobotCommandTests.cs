using Moq;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Models;
using Xunit;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class MoveRobotCommandTests
    {
        private readonly Mock<IRobot> _robot;
        private readonly Table _table;

        public MoveRobotCommandTests()
        {
            _robot = new Mock<IRobot>();
            _table = new Table() { XDimension = 5, YDimension = 5 };
        }

        [Fact]
        public void Execute_ThrowsException_WhenRobotNotPlaced()
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(false);
            var command = new MoveRobotCommand<IRobot>();

            Assert.Throws<RobotCommandException>(() => command.ValidateAndExecute(_robot.Object, _table));
        }

        [Fact]
        public void Execute_ThrowsException_WhenRobotIsAtTableEdge()
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(false);
            var command = new MoveRobotCommand<IRobot>();

            Assert.Throws<RobotCommandException>(() => command.ValidateAndExecute(_robot.Object, _table));
        }

        [Fact]
        public void Execute_ThrowsException_WhenRobotWouldMoveOffTableEdge()
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(true);
            _robot.SetupGet(r => r.Position).Returns(new RobotPosition { X = 2, Y = 4 });
            _robot.SetupGet(r => r.Facing).Returns(CompassDirection.North);
            var command = new MoveRobotCommand<IRobot>();

            Assert.Throws<RobotCommandException>(() => command.ValidateAndExecute(_robot.Object, _table));
        }

        [ClassData(typeof(Execute_MovesRobotToNextPosition_TestsData))]
        [Theory]
        public void Execute_MovesRobotToNextPosition(RobotPosition expectedNextPosition, CompassDirection facing)
        {
            _robot.SetupGet(r => r.IsPlaced).Returns(true);
            _robot.SetupGet(r => r.Position).Returns(new RobotPosition {X = 2, Y = 2});
            _robot.SetupGet(r => r.Facing).Returns(facing);

            var command = new MoveRobotCommand<IRobot>();

            command.ValidateAndExecute(_robot.Object, _table);
            _robot.VerifySet(r => r.Position = It.Is<RobotPosition>(robotPosition => robotPosition.X.Equals(expectedNextPosition.X) && robotPosition.Y.Equals(expectedNextPosition.Y)), Times.Once());
        }

        private class Execute_MovesRobotToNextPosition_TestsData : TheoryData<RobotPosition, CompassDirection>
        {
            public Execute_MovesRobotToNextPosition_TestsData()
            {
                Add(new RobotPosition { X = 2, Y = 3 }, CompassDirection.North);
                Add(new RobotPosition { X = 3, Y = 2 }, CompassDirection.East);
                Add(new RobotPosition { X = 2, Y = 1 }, CompassDirection.South);
                Add(new RobotPosition { X = 1, Y = 2 }, CompassDirection.West);
            }
        }
    }
}
