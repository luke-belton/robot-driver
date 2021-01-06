namespace ToyRobotSimulator.Models
{
    public class Table
    {
        public int XDimension { get; set; }
        public int YDimension { get; set; }

        public bool ContainsCoordinate(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < XDimension && y < YDimension);
        }
    }
}
