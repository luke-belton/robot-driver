using System;
using ToyRobotSimulator.Exceptions;
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
                try
                {
                    var rawCommand = Console.ReadLine();
                    var command = commandManager.GetCommandFromInput(rawCommand);
                    robot = controller.ExecuteSingleCommand(command, robot);
                }
                catch (RobotCommandException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Something went really wrong with the Robot - oops!");
                    Console.ResetColor();
                    break;
                }
            }
        }
    }
}
