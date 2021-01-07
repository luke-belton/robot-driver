using System;
using System.Collections.Generic;
using System.Linq;
using ToyRobotSimulator.Constants;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Helpers.RobotCommands;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers
{
    public interface ICommandManager<TRobot> where TRobot : IRobot
    {
        /// <summary>
        /// Parse input string to a command
        /// </summary>
        /// <param name="inputCommand"></param>
        /// <returns>An implementation of <see cref="IRobotCommand{TRobot}"/> depending on input</returns>
        /// <exception cref="RobotCommandException"> thrown when a problem is encountered with the supplied input.</exception>
        public IRobotCommand<TRobot> GetCommandFromInput(string inputCommand);

        /// <summary>
        /// Parse text file to list of robot commands
        /// </summary>
        /// <param name="filePath">Path to text file containing commands</param>
        /// <returns>An implementation of <see cref="IRobotCommand{TRobot}"/> depending on input</returns>
        /// <exception cref="RobotCommandException"> thrown when a problem is encountered with the supplied commands.</exception>
        public List<IRobotCommand<TRobot>> GetCommandsFromFile(string filePath);
    }
    public class CommandManager<TRobot> : ICommandManager<TRobot> where TRobot : IRobot
    {
        public IRobotCommand<TRobot> GetCommandFromInput(string inputCommand)
        {
            if (string.IsNullOrEmpty(inputCommand))
            {
                throw new RobotCommandException($"A command must be supplied in order to control the robot.");
            }

            var command = inputCommand.Split(" ");
            switch (command.First())
            {
                case Commands.Place:
                    if (command.ElementAtOrDefault(1) == null)
                        throw new RobotCommandException(
                            $"The command `{inputCommand}` was not supplied with a valid position.");
                    var (x, y, facing) = GetPlacementCommand(command[1]);
                    return new PlaceRobotCommand<TRobot>(x, y, facing);
                case Commands.Move:
                    CheckSingleCommand(command);
                    return new MoveRobotCommand<TRobot>();
                case Commands.Left:
                    CheckSingleCommand(command);
                    return new TurnRobotCommand<TRobot>(TurnDirection.Left);
                case Commands.Right:
                    CheckSingleCommand(command);
                    return new TurnRobotCommand<TRobot>(TurnDirection.Right);
                case Commands.Report:
                    CheckSingleCommand(command);
                    return new ReportRobotCommand<TRobot>();
                default:
                    throw new RobotCommandException($"The command `{command.First()}` is not recognised as a valid command.");
            }
        }

        public List<IRobotCommand<TRobot>> GetCommandsFromFile(string filePath)
        {
            var file = new System.IO.StreamReader(filePath);
            string line;
            var commands = new List<IRobotCommand<TRobot>>();
            while ((line = file.ReadLine()) != null)
            {
                commands.Add(GetCommandFromInput(line));
            }
            file.Close();

            return commands;
        }

        private void CheckSingleCommand(string[] commands)
        {
            if (commands.Length != 1)
            {
                throw new RobotCommandException($"Command `{commands.First()}` must not be accompanied by any other instructions.");
            }
        }

        private (int x, int y, CompassDirection facing) GetPlacementCommand(string input)
        {
            var rawCommands = input.Split(",");
            try
            {
                var x = int.Parse(rawCommands[0]);
                var y = int.Parse(rawCommands[1]);
                return rawCommands[2] switch
                {
                    (FaceDirection.North) => (x, y, CompassDirection.North),
                    (FaceDirection.East) => (x, y, CompassDirection.East),
                    (FaceDirection.South) => (x, y, CompassDirection.South),
                    (FaceDirection.West) => (x, y, CompassDirection.West),
                    _ => throw new RobotCommandException($"Did not recognise the compass direction {rawCommands[2]}")
                };
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(RobotCommandException))
                {
                    throw;
                }
                throw new RobotCommandException($"Something went wrong trying to get the placement command from the {Commands.Place} command input: `{input}`");
            }
        }
    }
}
