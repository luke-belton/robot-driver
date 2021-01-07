using System.Collections.Generic;
using System.Windows.Input;
using FluentAssertions;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Helpers;
using ToyRobotSimulator.Helpers.RobotCommands;
using ToyRobotSimulator.Models;
using Xunit;

namespace ToyRobotSimulator.IntegrationTests
{
    public class IntegrationTests
    {
        private readonly IRobotController<IRobot> _robotController;

        public IntegrationTests()
        {
            var table = new Table {XDimension = 5, YDimension = 5};
            _robotController = new RobotController<IRobot>(table);
        }

        [ClassData(typeof(RobotController_ShouldPassTestCases_TestsData))]
        [Theory]
        public void RobotController_ShouldPassTestCases(List<IRobotCommand<IRobot>> commands, IRobot expectedRobot)
        {
            IRobot robot = new ToyRobot();
            foreach (var command in commands)
            {
                _robotController.AddCommand(command);
            }

            robot = _robotController.ExecuteCommands(robot);
            robot.Should().BeEquivalentTo(expectedRobot);
        }

        private class RobotController_ShouldPassTestCases_TestsData : TheoryData<List<IRobotCommand<IRobot>>, IRobot>
        {
            public RobotController_ShouldPassTestCases_TestsData()
            {
                var commandManager = new CommandManager<IRobot>();
                // first test case
                var firstCommands = new List<IRobotCommand<IRobot>>()
                {
                    commandManager.GetCommandFromInput("PLACE 0,0,NORTH"),
                    commandManager.GetCommandFromInput("MOVE"),
                    commandManager.GetCommandFromInput("REPORT")
                };
                var firstExpectedRobot = new ToyRobot()
                    {Facing = CompassDirection.North, IsPlaced = true, Position = new RobotPosition {X = 0, Y = 1}};
                Add(firstCommands, firstExpectedRobot);

                // second test case
                var secondCommands = new List<IRobotCommand<IRobot>>
                {
                    commandManager.GetCommandFromInput("PLACE 0,0,NORTH"),
                    commandManager.GetCommandFromInput("LEFT"),
                    commandManager.GetCommandFromInput("REPORT")
                };
                var secondExpectedRobot = new ToyRobot()
                    { Facing = CompassDirection.West, IsPlaced = true, Position = new RobotPosition { X = 0, Y = 0 } };
                Add(secondCommands, secondExpectedRobot);

                // third test case
                var thirdCommands = new List<IRobotCommand<IRobot>>
                {
                    commandManager.GetCommandFromInput("PLACE 1,2,EAST"),
                    commandManager.GetCommandFromInput("MOVE"),
                    commandManager.GetCommandFromInput("MOVE"),
                    commandManager.GetCommandFromInput("LEFT"),
                    commandManager.GetCommandFromInput("MOVE"),
                    commandManager.GetCommandFromInput("REPORT")
                };
                var thirdExpectedRobot = new ToyRobot()
                    { Facing = CompassDirection.North, IsPlaced = true, Position = new RobotPosition { X = 3, Y = 3 } };
                Add(thirdCommands, thirdExpectedRobot);
            }
        }

    }
}
