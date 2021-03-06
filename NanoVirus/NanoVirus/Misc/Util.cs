﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NanoVirus
{
    /// <summary>
    /// Contains various commonly used functions
    /// </summary>
    public static class Util
    {

        /// <summary>
        /// Calculates the distance between two cells
        /// </summary>
        /// <param name="c1">Position of the first cell</param>
        /// <param name="c2">Position of the second cell</param>
        /// <returns>The distance between the cells</returns>
        public static int CalculateDistance(Position c1, Position c2)
        {
            double xDifference = Math.Pow((c1.X - c2.X), 2);
            double yDifference = Math.Pow((c1.Y - c2.Y), 2);
            double zDifference = Math.Pow((c1.Z - c2.Z), 2);
            double total = xDifference + yDifference + zDifference;
            double distance = Math.Sqrt(total);
            return (int)Math.Floor(distance);
        }

    }
}