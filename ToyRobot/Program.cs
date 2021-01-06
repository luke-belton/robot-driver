using System;
using ToyRobotSimulator.Helpers;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Toy Robot Simulator!");
            var commandManager = new CommandManager<ToyRobot>();
            var controller = new RobotController<ToyRobot>(new Table {XDimension = 5, YDimension = 5 });
            var robot = new ToyRobot();
            while (true)
            {
                var rawCommand = Console.ReadLine();
                var command = commandManager.GetCommandFromInput(rawCommand);
                robot = controller.ExecuteSingleCommand(command, robot);
            }
        }
    }
}
