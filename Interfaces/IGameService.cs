using PacManWpfApp.Models;
using System.Collections.Generic;

namespace PacManWpfApp.Interfaces
{
    public interface IGameService
    {
        PacMan PacMan { get; }
        List<Ghost> Ghosts { get; }
        List<Pellet> Pellets { get; }
        List<Wall> Walls { get; }
        int Lives { get; }
        int PalletsNumber { get; }
        void InitializeGame(Level level);
        void UpdateGame();
        void HandleInput(Direction direction);
    }
}
