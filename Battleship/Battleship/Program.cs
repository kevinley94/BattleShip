using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid();
            Console.WriteLine("You are the admiral of a Super Star Destroyer.\nYour particular ship comes equipped with a powerful artillery shell.\nThe drawback is each shot takes a full day to reload.\nYour duty is to wipe out the rebel scum.\nPress enter to continue...");
            Console.ReadKey();
            Console.Clear();
            grid.PlayGame();
            Console.ReadKey();
        }
    }
    class Point
    {
        public enum PointStatus
        {
            Empty,
            Ship,
            Hit,
            Miss,
            Event,
            Investigated
        }
        public int X { get; set; }
        public int Y { get; set; }
        public PointStatus Status { get; set; }

        public Point(int x, int y, PointStatus p)
        {
            this.X = x;
            this.Y = y;
        }
    }
    class Ship
    {
        public enum ShipType
        {
            Carrier,
            Battleship,
            Cruiser,
            Submarine,
            Minesweeper
        }
        public ShipType Type { get; set; }
        public List<Point> OccupiedPoints { get; set; }
        public int Length { get; set; }
        public bool IsDestroyed
        {
            get
            {
                bool hit = false;
                foreach (Point item in OccupiedPoints)
                {
                    if (item.Status == Point.PointStatus.Hit)
                    {
                        hit = true;
                    }
                    else
                    {
                        hit = false;
                        break;
                    }
                } return hit;
            }
        }
        public Ship(ShipType TypeOfShip)
        {
            OccupiedPoints = new List<Point>();
            this.Type = TypeOfShip;
            switch (TypeOfShip)
            {
                case ShipType.Carrier:
                    this.Length = 5;
                    break;
                case ShipType.Battleship:
                    this.Length = 4;
                    break;
                case ShipType.Cruiser:
                    this.Length = 3;
                    break;
                case ShipType.Submarine:
                    this.Length = 3;
                    break;
                case ShipType.Minesweeper:
                    this.Length = 2;
                    break;
                default:
                    break;
            }
        }
    }
    class Grid
    {
        public enum PlaceShipDirection
        {
            Horizontal,
            Vertical
        }
        public Point[,] Ocean { get; set; }
        public List<Ship> ListOfShips { get; set; }
        public bool AllShipsDestroyed
        {
            get
            {
                bool destroyed = false;
                foreach (Ship ship in ListOfShips)
                {
                    if (ship.IsDestroyed)
                    {
                        destroyed = true;
                        Console.WriteLine("You have destroyed a {0}", ship.Type);
                    }
                    else { destroyed = false; break; }
                }
                return destroyed;
            }
        }
        public int CombatRound { get; set; }
        public Grid()
        {
            this.Ocean = new Point[10, 10];
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    this.Ocean[x, y] = new Point(x, y, Point.PointStatus.Empty);
                }
            }
            Ship Battleship = new Ship(Ship.ShipType.Battleship);
            Ship Carrier = new Ship(Ship.ShipType.Cruiser);
            Ship Cruiser = new Ship(Ship.ShipType.Cruiser);
            Ship Minesweeper = new Ship(Ship.ShipType.Minesweeper);
            Ship Submarine = new Ship(Ship.ShipType.Submarine);
            PlaceShip(Battleship, PlaceShipDirection.Horizontal, 0, 0);
            PlaceShip(Carrier, PlaceShipDirection.Horizontal, 0, 1);
            PlaceShip(Cruiser, PlaceShipDirection.Horizontal, 0, 2);
            PlaceShip(Minesweeper, PlaceShipDirection.Horizontal, 0, 3);
            PlaceShip(Submarine, PlaceShipDirection.Horizontal, 0, 4);

            ListOfShips = new List<Ship>();
            ListOfShips.Add(Battleship);
            ListOfShips.Add(Carrier);
            ListOfShips.Add(Cruiser);
            ListOfShips.Add(Minesweeper);
            ListOfShips.Add(Submarine);



        }
        public void PlaceShip(Ship shipToPlace, PlaceShipDirection direction, int startX, int startY)
        {
            for (int i = 0; i < shipToPlace.Length; i++)
            {
                Ocean[startX, startY].Status = Point.PointStatus.Ship;
                shipToPlace.OccupiedPoints.Add(Ocean[startX, startY]);
                if (direction == PlaceShipDirection.Horizontal)
                {
                    startX++;
                }
                else { startY++; }

            }
        }
        public void DisplayOcean()
        {
            Console.Write("  0  1  2  3  4  5  6  7  8  9\n");

            for (int y = 0; y < 10; y++)
            {
                Console.Write(y + "");

                for (int x = 0; x < 10; x++)
                {
                    switch (this.Ocean[x, y].Status)
                    {
                        case Point.PointStatus.Empty:
                            Console.Write("[ ]");
                            break;
                        case Point.PointStatus.Hit:
                            Console.Write("[X]");
                            break;
                        case Point.PointStatus.Miss:
                            Console.Write("[O]");
                            break;
                        case Point.PointStatus.Ship:
                            Console.Write("[ ]");
                            break;
                        case Point.PointStatus.Event:
                            Console.Write("[?]");
                            break;
                        case Point.PointStatus.Investigated:
                            Console.Write("[*]");
                            break;
                    }

                } Console.WriteLine();
                
            }Console.WriteLine("Days Passed: {0}", CombatRound);
        }
        public bool Target(int x, int y)
        {
            bool HitOrMiss = false;
            int numberOfShipsDestroyed = ListOfShips.Where(k => k.IsDestroyed).Count();
            Point Target = Ocean[x, y];
            if (Target.Status == Point.PointStatus.Ship)
            {
                Target.Status = Point.PointStatus.Hit;
                Console.WriteLine("HIT");
                Console.WriteLine("Press enter to continue..");
                Console.ReadKey();
                
            }
            else if (Target.Status == Point.PointStatus.Empty)
            {
                Target.Status = Point.PointStatus.Miss;
                Console.WriteLine("MISS");
                Console.WriteLine("Press enter to continue..");
                Console.ReadKey();
                
            }
            
            int newNumberOfShipsDestroyed = ListOfShips.Where(k => k.IsDestroyed).Count();
            
            if (newNumberOfShipsDestroyed > numberOfShipsDestroyed)
            {
               return true;
            }
            return false;
        }
        public void PlayGame()
        {
            do
            {
                this.DisplayOcean();
                Console.WriteLine("Enter an X coordinate:");
                int inputX = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter a Y coordinate:");
                int inputY = int.Parse(Console.ReadLine());
                Target(inputX, inputY);
                
                
                CombatRound = CombatRound + 1;

                Console.Clear();

            } while (AllShipsDestroyed == false);
            Console.WriteLine("The Rebel Fleet lays dead before you, it took you: {0} days to win", CombatRound);
            Endings(CombatRound);
        }
        public void Endings(int rounds)
        {
            if (rounds < 17)
            {
                Console.WriteLine("Your accuracy is peerless. Darth Vader himself has become impressed with your efficiency.");
            }
            else if (rounds > 17 && rounds < 30)
            {
                Console.WriteLine("Through careful toil, you eventually destroy the rebels.\nHowever your ship's cannon has been destroyed,\npreventing you from participating in the coming invasion.");
            }
            else if (rounds > 30)
            {
                Console.WriteLine("You finally destroy the rebels after a long and drawn out battle. Darth Vader is displeased.\n As he pulls you up on his screen, you begin to feel like you're suffocating\nYou have failed him for the last time.");
            }
        }
        public void Random(Point target)
        {
            Random rng = new Random();
            int newX = rng.Next(0, 10);
            int newY = rng.Next(0,10);
        }
    }
    
}