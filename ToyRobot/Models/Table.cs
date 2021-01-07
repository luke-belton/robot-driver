namespace ToyRobotSimulator.Models
{
    public class Table
    {
        /// <summary>
        /// The number of units along the x dimension
        /// </summary>
        public int XDimension { get; set; }

        /// <summary>
        /// The number of units along the y dimension
        /// </summary>
        public int YDimension { get; set; }

        /// <summary>
        /// Checks whether the table dimensions contain the supplied coordinate
        /// </summary>
        /// <param name="x">X coordinate to check</param>
        /// <param name="y">Y coordinate to check</param>
        /// <returns>Boolean indicating whether the coordinate is within the table</returns>
        public bool ContainsCoordinate(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < XDimension && y < YDimension);
        }
    }
}
