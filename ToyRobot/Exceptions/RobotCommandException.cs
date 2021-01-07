using System;

namespace ToyRobotSimulator.Exceptions
{
    public class RobotCommandException : Exception
    {
        /// <summary>
        /// Thrown when an incorrectly formatted or unsuitable command is given to a robot
        /// </summary>
        /// <param name="message">Message describing error encountered</param>
        public RobotCommandException(string message) : base(message) { }
    }
}
