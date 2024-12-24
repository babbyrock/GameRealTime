using GameProject.Domain.Models;

namespace GameProject.Application.Services
{
    public class GameSessionService
    {
        private readonly List<Player> _players;

        public GameSessionService()
        {
            _players = new List<Player>();
        }

        public async Task<Player> AddPlayer(string name)
        {
            var player = new Player
            {
                Id = _players.Count + 1, 
                Name = name
            };

            _players.Add(player);

            return player; 
        }

        public List<Player> GetPlayers()
        {
            return _players;
        }

        public void ResetPlayers()
        {
            _players.Clear(); // Limpa todos os jogadores
        }
    }
}
