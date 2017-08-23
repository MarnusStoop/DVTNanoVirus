using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NanoVirus
{
    public class Cell
    {
        private Position position;
        private CellType cellType;

        public Position Position
        {
            get { return position; }
            set { position = value; }
        }

        public CellType CellType
        {
            get { return cellType; }
            set { cellType = value; }
        }

        public Cell(Position position, CellType cellType)
        {
            this.Position = position;
            this.CellType = cellType;
        }

        public override string ToString()
        {
            return string.Format("{0} Type:{1}", Position.ToString(), CellType.ToString());
        }

    }
}