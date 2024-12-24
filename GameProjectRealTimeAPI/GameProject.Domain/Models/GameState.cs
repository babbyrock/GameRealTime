using System.Collections.Generic;

namespace GameProject.Domain.Models
{
    public class GameState
    {
        public required List<Player> Players { get; set; }
        public required Player CurrentPlayer { get; set; } 
        public int CurrentTurnIndex { get; set; } 
        public bool GameStarted { get; set; } 
        public bool ContagemIniciada { get; set; }
        public int TotalTurns { get; set; } 
        public string? Status { get; set; }
    }
}
