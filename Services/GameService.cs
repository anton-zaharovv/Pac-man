using PacManWpfApp.Interfaces;
using PacManWpfApp.Models;
using System;
using System.Collections.Generic;

namespace PacManWpfApp.Services
{
    public class GameService : IGameService
    {
        private readonly Random _random = new Random();
        private const double InitialPacManX = 100;
        private const double InitialPacManY = 100;

        public PacMan PacMan { get; private set; }
        public List<Ghost> Ghosts { get; private set; }
        public List<Pellet> Pellets { get; private set; }
        public List<Wall> Walls { get; private set; }

        public int Lives { get; private set; }

        public int PalletsNumber { get; private set; }

        public void InitializeGame(Level level)
        {
            PacMan = new PacMan { X = InitialPacManX, Y = InitialPacManY, Speed = 5, Lives = 3 };
            Ghosts = new List<Ghost>(level.Ghosts);
            Pellets = new List<Pellet>(level.Pellets);
            Walls = new List<Wall>(level.Walls);

            Lives = 3;
            PalletsNumber = 0;
        }

        public void UpdateGame()
        {
            PacMan.Move();
            foreach (var ghost in Ghosts)
            {
                ghost.Move();
            }

            CheckCollisions();
        }

        public void HandleInput(Direction direction)
        {
            PacMan.Direction = direction;
        }

        private void CheckCollisions()
        {
            // Check for collisions with pellets
            foreach (var pellet in Pellets)
            {
                if (!pellet.IsEaten && CheckCollision(PacMan, pellet))
                {
                    pellet.IsEaten = true;
                    PalletsNumber++;
                    // Increase score or any other logic
                }
            }

            // Check for collisions with walls
            foreach (var wall in Walls)
            {
                if (CheckCollision(PacMan, wall))
                {
                    HandleWallCollision(PacMan, wall);
                }
                foreach (var ghost in Ghosts)
                {
                    if (CheckCollision(ghost, wall))
                    {
                        HandleWallCollision(ghost, wall);
                        ChangeGhostDirection(ghost);
                    }
                }
            }

            // Check for collisions with ghosts
            foreach (var ghost in Ghosts)
            {
                if (CheckCollision(PacMan, ghost))
                {
                    PacMan.Lives--;
                    if (PacMan.Lives > 0)
                    {
                        PacMan.Respawn(InitialPacManX, InitialPacManY);
                    }
                    else
                    {
                        // Handle game over logic here
                    }
                }
            }
        }

        private bool CheckCollision(GameObject obj1, GameObject obj2)
        {
            return obj1.X < obj2.X + 20 && obj1.X + 20 > obj2.X &&
                   obj1.Y < obj2.Y + 20 && obj1.Y + 20 > obj2.Y;
        }

        private bool CheckCollision(GameObject obj, Pellet pellet)
        {
            return obj.X < pellet.X + 10 && obj.X + 20 > pellet.X &&
                   obj.Y < pellet.Y + 10 && obj.Y + 20 > pellet.Y;
        }

        private bool CheckCollision(GameObject obj, Wall wall)
        {
            return obj.X < wall.X + wall.Width && obj.X + 20 > wall.X &&
                   obj.Y < wall.Y + wall.Height && obj.Y + 20 > wall.Y;
        }

        private void HandleWallCollision(GameObject obj, Wall wall)
        {
            switch (obj.Direction)
            {
                case Direction.Up:
                    obj.Y = wall.Y + wall.Height;
                    break;
                case Direction.Down:
                    obj.Y = wall.Y - 20;
                    break;
                case Direction.Left:
                    obj.X = wall.X + wall.Width;
                    break;
                case Direction.Right:
                    obj.X = wall.X - 20;
                    break;
            }
        }

        private void ChangeGhostDirection(Ghost ghost)
        {
            Direction originalDirection = ghost.Direction;
            Direction newDirection;

            do
            {
                newDirection = (Direction)_random.Next(4);
            }
            while (newDirection == originalDirection);

            ghost.Direction = newDirection;
        }
    }

}
