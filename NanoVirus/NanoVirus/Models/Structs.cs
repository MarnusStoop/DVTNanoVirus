using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NanoVirus
{
    public struct Position
    {
        private int x;
        private int y;
        private int z;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} Z:{2}", X, Y, Z);
        }

        public bool SamePosition(Position other)
        {
            return other.X == X && other.Y == Y && other.Z == Z;
        }

    }

    public struct CellDistance
    {
        public Cell cell;
        public int distance;

        public CellDistance(Cell cell,int distance)
        {
            this.cell = cell;
            this.distance = distance;
        }
    }

}