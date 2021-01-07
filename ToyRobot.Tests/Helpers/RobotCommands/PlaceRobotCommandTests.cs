using Moq;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Models;
using Xunit;

namespace ToyRobotSimulator.Helpers.RobotCommands
{
    public class PlaceRobotCommandTests
    {
        private readonly Mock<IRobot> _robot;
        private readonly Table _table;

        public PlaceRobotCommandTests()
        {
            _robot = new Mock<IRobot>();
            _table = new Table() { XDimension = 5, YDimension = 5 };
        }

        [Fact]
        public void Execute_ThrowsException_WhenRobotPlacedOutsideTable()
        {
            var command = new PlaceRobotCommand<IRobot>(10, 20, CompassDirection.North);

            Assert.Throws<RobotCommandException>(() => command.ValidateAndExecute(_robot.Object, _table));
        }

        [Fact]
        public void Execute_ThrowsException_WhenRobotCoordinatesAreNegative()
        {
            var command = new PlaceRobotCommand<IRobot>(-1, -1, CompassDirection.North);

            Assert.Throws<RobotCommandException>(() => command.ValidateAndExecute(_robot.Object, _table));
        }

        [Fact]
        public void Execute_PlacesRobot_WhenValidPositionSupplied()
        {
            var command = new PlaceRobotCommand<IRobot>(2, 3, CompassDirection.North);

            command.ValidateAndExecute(_robot.Object, _table);

            _robot.VerifySet(r =>
                    r.Position = It.Is<RobotPosition>(robotPosition =>
                        robotPosition.X.Equals(2) && robotPosition.Y.Equals(3)),
                Times.Once);
            _robot.VerifySet(r => r.Facing = CompassDirection.North, Times.Once);
            _robot.VerifySet(r => r.IsPlaced = true, Times.Once);
        }
    }
}
