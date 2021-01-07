using System;
using System.Collections.Generic;
using ToyRobotSimulator.Exceptions;
using ToyRobotSimulator.Helpers.RobotCommands;
using ToyRobotSimulator.Models;

namespace ToyRobotSimulator.Helpers
{
    public interface IRobotController<TRobot> where TRobot : IRobot
    {
        public Table Table { get; set; }

        public List<IRobotCommand<TRobot>> Commands { get; }

        public void AddCommand<TCommand>(TCommand command) where TCommand : IRobotCommand<TRobot>;

        public TRobot ExecuteCommands(TRobot robot);

        public TRobot ExecuteSingleCommand<TCommand>(TCommand command, TRobot robot)
            where TCommand : IRobotCommand<TRobot>;
    }
    
    public class RobotController<TRobot> : IRobotController<TRobot> where TRobot : IRobot
    {
        public Table Table { get; set; }

        public List<IRobotCommand<TRobot>> Commands { get; } = new List<IRobotCommand<TRobot>>();

        public RobotController(Table table)
        {
            Table = table;
        }

        public void AddCommand<TCommand>(TCommand command) where TCommand : IRobotCommand<TRobot>
        {
            Commands.Add(command);
        }

        public TRobot ExecuteCommands(TRobot robot)
        {
            foreach (var command in Commands)
            {
                try
                {
                    robot = command.ValidateAndExecute(robot, Table);
                }
                catch (RobotCommandException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            return robot;
        }

        public TRobot ExecuteSingleCommand<TCommand>(TCommand command, TRobot robot)
            where TCommand : IRobotCommand<TRobot>
        {
            robot = command.ValidateAndExecute(robot, Table);
            return robot;
        }
    }
}
