using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NanoVirus
{
    /// <summary>
    /// The body that contains the algorithm and cell data
    /// </summary>
    class Body
    {
        List<Cell> allCells;
        List<Cell> redBloodCells;
        List<Cell> whiteBloodCells;
        List<Cell> tumorousCells;
        Virus nanoVirus;
        VirusAction previousAction;

        Random r = new Random();

        int cycleCounter = 0;
        int tumorousAttackValue = 5;
        int healthyCellsRemaining = 0;
        int tumorousCellsRemaining = 0;
        int amountOfCells = 100;
        string appStateFilePath = "State.txt";

        public Body()
        {

        }

        public Body(int amountOfCells,int tumorAttackValue,string appStateFilePath)
        {
            this.amountOfCells = amountOfCells;
            this.tumorousAttackValue = tumorAttackValue;
            this.appStateFilePath = appStateFilePath;
        }

        /// <summary>
        /// Generates the cells and virus
        /// </summary>
        public void GenerateBody()
        {
            allCells = new List<Cell>();
            for (int i = 0; i < 100; i++)
            {
                Position cellPosition = GenerateRandomPosition();
                CellType typeOfCell = GenerateRandomCellType();
                Cell newCell = new Cell(cellPosition, typeOfCell);
                allCells.Add(newCell);
            }
            CalculateCellStats();
            Cell startingCell = SelectStartingRedBloodCell();
            nanoVirus = new Virus(startingCell.Position);
        }

        /// <summary>
        /// Assigns the various cells to lists and calculates how many cells remain
        /// </summary>
        private void CalculateCellStats()
        {
            redBloodCells = new List<Cell>();
            whiteBloodCells = new List<Cell>();
            tumorousCells = new List<Cell>();
            whiteBloodCells = (from c in allCells
                               where c.CellType == CellType.WhiteBloodCell
                               select c).ToList();
            redBloodCells = (from c in allCells
                             where c.CellType == CellType.RedBloodCell
                             select c).ToList();
            tumorousCells = (from c in allCells
                             where c.CellType == CellType.Tumorous
                             select c).ToList();
            healthyCellsRemaining = whiteBloodCells.Count + redBloodCells.Count;
            tumorousCellsRemaining = tumorousCells.Count;
        }

        /// <summary>
        /// Generates a random position for the cells
        /// </summary>
        /// <returns>A position with x,y and z values between 1 and 5000</returns>
        private Position GenerateRandomPosition()
        {
            int x = r.Next(1, 5001);
            int y = r.Next(1, 5001);
            int z = r.Next(1, 5001);

            return new Position(x, y, z);
        }

        /// <summary>
        /// Generates a random cell type with the probabilites for tumorous,red and white cells of 5,70 and 25 respectively
        /// </summary>
        /// <returns></returns>
        private CellType GenerateRandomCellType()
        {
            int rolled = r.Next(1, 101);
            if (rolled <= 5)
            {
                return CellType.Tumorous;
            } else if (rolled > 5 && rolled <= 30)
            {
                return CellType.WhiteBloodCell;
            } else if (rolled > 30)
            {
                return CellType.RedBloodCell;
            }
            return CellType.RedBloodCell;
        }

        /// <summary>
        /// Selects a random red blood cell for the virus to spawn on
        /// </summary>
        /// <returns>The selected red blood cell</returns>
        private Cell SelectStartingRedBloodCell()
        {
            int cellSelected = r.Next(0, redBloodCells.Count);
            return redBloodCells[cellSelected];
        }

        /// <summary>
        /// Starts to run the various algorithms and checks whether the game is over
        /// </summary>
        public void StartVirus()
        {
            while (tumorousCellsRemaining > 0 && healthyCellsRemaining > 0)
            {
                if (cycleCounter % tumorousAttackValue == 0 && cycleCounter != 0)
                {
                    SpreadTumorousCells();
                }
                ActivateVirus();
                cycleCounter++;
                FileHandler.Append(FormatCycleDetails(), appStateFilePath);
            }
            FileHandler.Append(FormatCycleDetails(), appStateFilePath);
            if (tumorousCellsRemaining == 0)
            {
                Console.WriteLine("The virus won");
                FileHandler.Append("\nThe virus won", appStateFilePath);
                return;
            }
            if (healthyCellsRemaining == 0)
            {
                Console.WriteLine("The tumors won");
                FileHandler.Append("\nThe tumors won", appStateFilePath);
                return;
            }
        }

        /// <summary>
        /// Activates the tumorous algorithm and infects nearby cells
        /// </summary>
        private void SpreadTumorousCells()
        {
            foreach (var item in tumorousCells)
            {
                Cell cellToInfect = FindNearestHealthyCell(item.Position);
                if (cellToInfect != null)
                {
                    allCells.Find(x => x == cellToInfect).CellType = CellType.Tumorous;
                }
                CalculateCellStats();
            }
        }

        /// <summary>
        /// Finds the nearest healthy cell, checks for red first and then white
        /// </summary>
        /// <param name="tumorousCellPos">The position of the tumorous cell</param>
        /// <returns>The nearest healthy cell</returns>
        private Cell FindNearestHealthyCell(Position tumorousCellPos)
        {
            Cell nearestCell = null;
            if (redBloodCells.Count <= 0)
            {
                List<CellDistance> distances = new List<CellDistance>();
                foreach (var item in whiteBloodCells)
                {
                    int distance = Util.CalculateDistance(item.Position, tumorousCellPos);
                    distances.Add(new CellDistance(item, distance));
                }
                if (distances.Count > 0)
                {
                    nearestCell = distances.OrderBy(x => x.distance).First().cell;
                }
            } else
            {
                List<CellDistance> distances = new List<CellDistance>();
                foreach (var item in redBloodCells)
                {
                    int distance = Util.CalculateDistance(item.Position, tumorousCellPos);
                    distances.Add(new CellDistance(item, distance));
                }
                if (distances.Count > 0)
                {
                    nearestCell = distances.OrderBy(x => x.distance).First().cell;
                }
            }
            return nearestCell;
        }

        /// <summary>
        /// Runs the virus algorithm and decides what to do
        /// </summary>
        private void ActivateVirus()
        {
            if (previousAction != VirusAction.Attack)
            {
                Cell currentCell = allCells.Find(x => x.Position.SamePosition(nanoVirus.CurrentPosition));
                if (currentCell.CellType == CellType.Tumorous)
                {
                    AttackCell(currentCell);
                    return;
                }
            }
            CellDistance nearestTumorousCell = FindNearestTumorousCell(nanoVirus.CurrentPosition);
            if (nearestTumorousCell.distance <= 5000)
            {
                MoveToCell(nearestTumorousCell.cell);
                return;
            } else if (nearestTumorousCell.distance > 5000)
            {
                CellDistance nearestHealthyCell = FindNearestHealthyCellToNearestTumorousCell(nearestTumorousCell.cell.Position);
                if (nearestHealthyCell.distance > 5000)
                {
                    previousAction = VirusAction.Nothing;
                    return;
                } else
                {
                    MoveToCell(nearestHealthyCell.cell);
                    return;
                }
            }
        }

        /// <summary>
        /// Moves the virus to a cell
        /// </summary>
        /// <param name="cellToMoveTo">The cell to move to</param>
        private void MoveToCell(Cell cellToMoveTo)
        {
            if (cellToMoveTo != null)
            {
                nanoVirus.CurrentPosition = cellToMoveTo.Position;
                previousAction = VirusAction.Move;
            }
        }

        /// <summary>
        /// Attacks and kills the current cell
        /// </summary>
        /// <param name="cellToKill">The cell to be attacked</param>
        private void AttackCell(Cell cellToKill)
        {
            if (cellToKill != null)
            {
                allCells.Remove(cellToKill);
                previousAction = VirusAction.Attack;
            }
            CalculateCellStats();
        }

        /// <summary>
        /// Finds the nearest tumorous cell
        /// </summary>
        /// <param name="virusPosition">The current position of the virus</param>
        /// <returns>The cell and distance of the nearest tumorous cell</returns>
        private CellDistance FindNearestTumorousCell(Position virusPosition)
        {
            CellDistance nearestCell = default(CellDistance);
            List<CellDistance> distances = new List<CellDistance>();
            foreach (var item in tumorousCells)
            {
                int distance = Util.CalculateDistance(item.Position, virusPosition);
                distances.Add(new CellDistance(item, distance));
            }
            if (distances.Count > 0)
            {
                nearestCell = distances.OrderBy(x => x.distance).First();
            }
            return nearestCell;
        }

        /// <summary>
        /// Finds the nearest healthy cell to the nearest tumorous cell
        /// </summary>
        /// <param name="tumorousCellPos">The position of the nearest tumorous cell</param>
        /// <returns>The cell and distance of the nearest cell</returns>
        private CellDistance FindNearestHealthyCellToNearestTumorousCell(Position tumorousCellPos)
        {
            CellDistance nearestCell = default(CellDistance);
            List<CellDistance> distances = new List<CellDistance>();
            foreach (var item in allCells)
            {
                if (item.CellType == CellType.Tumorous)
                {
                    continue;
                }
                int distance = Util.CalculateDistance(item.Position, tumorousCellPos);
                if (distance > 5000)
                {
                    continue;
                }
                distances.Add(new CellDistance(item, distance));
            }
            if (distances.Count > 0)
            {
                nearestCell = distances.OrderBy(x => x.distance).First();
            }
            return nearestCell;
        }

        /// <summary>
        /// Formats the current cycle's details so it can be written to a file
        /// </summary>
        /// <returns>The formatted details</returns>
        private string FormatCycleDetails()
        {
            return string.Format("Cycle: {0}\nRed blood cells: {1}\nWhite blood cells: {2}\nTumorous Cells: {3}\nVirus Action: {4}\n\n", cycleCounter, redBloodCells.Count, whiteBloodCells.Count, tumorousCellsRemaining, previousAction);
        }

    }
}