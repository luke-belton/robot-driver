using System;
using System.IO;
using System.Linq;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Helpers;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            GetInstructions();
            var commandManager = new CommandManager<ToyRobot>();
            var controller = new RobotController<ToyRobot>(new Table {XDimension = 5, YDimension = 5 });
            var robot = new ToyRobot();
            while (true)
            {
                try
                {
                    Console.WriteLine("Give the robot its next command: ");
                    var rawCommand = Console.ReadLine();
                    if (rawCommand != null && rawCommand.StartsWith(Commands.File))
                    {
                        var filePath = rawCommand.Split(" ").ElementAtOrDefault(1);
                        if (string.IsNullOrEmpty(filePath))
                        {
                            throw new RobotCommandException($"No filepath supplied with {Commands.File} command");
                        }

                        var rawCommands = commandManager.GetCommandsFromFile(filePath);
                        foreach (var command in rawCommands)
                        {
                            controller.AddCommand(command);
                        }

                        controller.ExecuteCommands(robot);
                        controller.ClearCommands();
                    }
                    else
                    {
                        var command = commandManager.GetCommandFromInput(rawCommand);
                        robot = controller.ExecuteSingleCommand(command, robot);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
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

        private static void GetInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"Welcome to the Toy Robot Simulator!

This Robot accepts the following instructions...

        PLACE X,Y,F - where X and Y are integers and F is NORTH/EAST/SOUTH/WEST
        MOVE - to advance the robot one position
        LEFT - to turn robot to the left by 90 degrees
        RIGHT - to turn robot to the right by 90 degrees
        REPORT - to report current position of robot in X,Y,F format as for PLACE command

You can upload a batch of instructions from a .txt file using the following method:
        
        FILE \path\to\file.txt

    ... each command must be given as above, with each command appearing on a new line.
    
The table is a 5 by 5 grid with origin at X=0, Y=0.
The first valid command is PLACE (any other command is ignored)
Any move that would position the robot off the table is ignored.
A second PLACE command can be given at any time to move the robot elsewhere on the table.

Have fun!

Start by using a PLACE command to put the robot on the table:"
            );
            Console.ResetColor();
        }
    }
}
