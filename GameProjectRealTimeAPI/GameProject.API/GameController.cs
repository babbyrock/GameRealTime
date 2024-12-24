using GameProject.Application.Services;
using GameProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GameProject.Application.Hubs;

namespace GameProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameSessionService _gameSessionService;
        private readonly GameService _gameService;
        private readonly IHubContext<GameHub> _hubContext;

        public GameController(GameSessionService gameSessionService, 
            GameService gameService, 
            IHubContext<GameHub> hubContext)
        {
            _gameSessionService = gameSessionService;
            _gameService = gameService;
            _hubContext = hubContext;
        }
        [HttpPost("register")]
        public async Task<IActionResult> AddPlayer([FromBody] Player player)
        {
            var addedPlayer = await _gameSessionService.AddPlayer(player.Name);

            var players = _gameSessionService.GetPlayers();
            await _hubContext.Clients.All.SendAsync("PlayersUpdated", players);

            var gameState = _gameService.GetGameState();
            gameState.Players.Add(addedPlayer);
            await _hubContext.Clients.All.SendAsync("GameStateUpdated", gameState);

            return Ok(addedPlayer);
        }

    }
}
