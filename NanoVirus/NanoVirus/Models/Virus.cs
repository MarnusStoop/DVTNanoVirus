using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NanoVirus
{
    class Virus
    {

        private Position currentPosition;

        public Position CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; }
        }

        public Virus(Position position)
        {
            this.CurrentPosition = position;
        }
    }
}