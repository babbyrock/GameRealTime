using GameProject.Application.Services;
using GameProject.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameProject.Application.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameSessionService _gameSessionService;
        private readonly GameService _gameService;

        public GameHub(GameSessionService gameSessionService, GameService gameService)
        {
            _gameSessionService = gameSessionService;
            _gameService = gameService;
        }

        public override async Task OnConnectedAsync()
        {
            var players = _gameSessionService.GetPlayers();
            await Clients.Caller.SendAsync("PlayersUpdated", players);

            var gameState = _gameService.GetGameState();
            await Clients.Caller.SendAsync("GameStateUpdated", gameState);
            await Clients.All.SendAsync("GameStatusUpdated", gameState.Status);
            await InitializeGame();

            await base.OnConnectedAsync();
        }

        private async Task InitializeGame()
        {
            try
            {
                var players = _gameSessionService.GetPlayers();
                var gameState = _gameService.GetGameState();

                if (players.Count > 1 && !gameState.GameStarted && !gameState.ContagemIniciada)
                {
                    _gameService.StartContagemIniciada();
                    await Clients.All.SendAsync("GameStatusUpdated", gameState.Status);
                    await Clients.All.SendAsync("GameStateUpdated", gameState);

                    int countdownTime = 30;

                    for (int i = countdownTime; i > 0; i--)
                    {
                        if (players.Count == 0)
                        {
                            countdownTime = 0;
                            await Clients.All.SendAsync("CountdownUpdated", null);
                            return;
                        }

                        await Clients.All.SendAsync("CountdownUpdated", i);
                        await Task.Delay(1000);
                    }

                    _gameService.StartGame();
                    gameState = _gameService.GetGameState();
                    await Clients.All.SendAsync("GameStateUpdated", gameState);
                    await Clients.All.SendAsync("GameStatusUpdated", gameState.Status);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public async Task AdvanceTurn()
        {
            try
            {
                _gameService.CheckForTimeLimit();
                _gameService.NextTurn();

                var gameState = _gameService.GetGameState();
                await Clients.All.SendAsync("GameStateUpdated", gameState);

                if (!gameState.GameStarted)
                {
                    await Clients.All.SendAsync("GameStatusUpdated", $"Jogo terminado! Apenas um jogador restou. {gameState.CurrentPlayer.Name} é o vencedor, Parabéns!");
                    return;
                }

                await Clients.All.SendAsync("GameStatusUpdated", $"Vez do jogador: {gameState.CurrentPlayer.Name}. Tempo acumulado: {gameState.CurrentPlayer.AccumulatedTime.TotalSeconds} segundos.");
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public async Task ResetGame()
        {
            try
            {
                _gameService.ResetGame();
                var gameState = _gameService.GetGameState();
                _gameSessionService.ResetPlayers();
                var players = _gameSessionService.GetPlayers();

                await Clients.All.SendAsync("GameStatusUpdated", "Jogo reiniciado! Atualizar página.");
                await Clients.All.SendAsync("GameStateUpdated", gameState);
                await Clients.All.SendAsync("PlayersUpdated", players);
                await Clients.All.SendAsync("CountdownUpdated", null);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }
    }
}
