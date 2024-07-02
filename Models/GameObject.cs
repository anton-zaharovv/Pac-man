using System;
using System.Collections.Generic;

namespace PacManWpfApp.Models
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public abstract class GameObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Speed { get; set; }
        public Direction Direction { get; set; }

        public void Move()
        {
            switch (Direction)
            {
                case Direction.Up:
                    Y -= Speed;
                    break;
                case Direction.Down:
                    Y += Speed;
                    break;
                case Direction.Left:
                    X -= Speed;
                    break;
                case Direction.Right:
                    X += Speed;
                    break;
            }
        }
    }

    public class PacMan : GameObject
    {
        public int Lives { get; set; }
        public void Respawn(double startX, double startY)
        {
            X = startX;
            Y = startY;
        }
    }

    public class Ghost : GameObject
    {
    }

    public class Pellet
    {
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsEaten { get; set; }
    }

    public class Wall
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class Level
    {
        public string Name { get; set; }
        public List<Ghost> Ghosts { get; set; }
        public List<Pellet> Pellets { get; set; }
        public List<Wall> Walls { get; set; }
    }

    public class GameRecord
    {
        public string PlayerName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public int PelletsCollected { get; set; }
    }
}
