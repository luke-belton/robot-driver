using System;

namespace ToyRobotSimulator.Exceptions
{
    public class RobotCommandException : Exception
    {
        public RobotCommandException(string message) : base(message) { }
    }
}
