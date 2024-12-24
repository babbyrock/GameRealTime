using GameProject.Domain.Models;

namespace GameProject.Application.Services
{
    public class GameService
    {
        private readonly GameSessionService _gameSessionService;
        private List<Player> _turnOrder;
        private int _currentTurn;
        private bool _gameStarted;
        private bool _contagemIniciada;
        private DateTime _gameStartTime;
        private DateTime _lastActionTime;
        private readonly TimeSpan _timeLimit = TimeSpan.FromSeconds(30);

        public GameService(GameSessionService gameSessionService)
        {
            _gameSessionService = gameSessionService;
            _turnOrder = new List<Player>();
            _currentTurn = 0;
            _gameStarted = false;
            _contagemIniciada = false;
        }

        public void StartGame()
        {
            _gameStarted = true;
            _currentTurn = 0;
            _gameStartTime = DateTime.Now;
            _lastActionTime = DateTime.Now;
            _contagemIniciada = false;
        }

        public void StartContagemIniciada()
        {
            _contagemIniciada = true;
        }

        public void NextTurn()
        {
            var currentPlayer = _turnOrder[_currentTurn];
            var elapsedTime = DateTime.Now - _lastActionTime;
            currentPlayer.AccumulatedTime += elapsedTime;

            if (currentPlayer.AccumulatedTime > _timeLimit)
            {
                _turnOrder.Remove(currentPlayer);

                if (_turnOrder.Count == 1)
                {
                    _gameStarted = false;
                }
            }

            _currentTurn = (_currentTurn + 1) % _turnOrder.Count;
            _lastActionTime = DateTime.Now;
        }

        public void CheckForTimeLimit()
        {
            var elapsedTime = DateTime.Now - _lastActionTime;

            if (elapsedTime > _timeLimit)
            {
                var currentPlayer = _turnOrder[_currentTurn];
                _turnOrder.Remove(currentPlayer);

                if (_turnOrder.Count == 1)
                {
                    _gameStarted = false;
                }
            }
        }

        public GameState GetGameState()
        {
            var currentPlayer = _turnOrder.Count > 0 ? _turnOrder[_currentTurn] : null;

            string statusMessage = _gameStarted switch
            {
                false when _contagemIniciada => "Iniciando contagem regressiva...",
                false => "Aguardando jogadores para iniciar.",
                true when _turnOrder.Count > 1 => $"Jogo em andamento! Vez do jogador: {currentPlayer?.Name}",
                _ => "Estado desconhecido."
            };

            return new GameState
            {
                Players = _turnOrder,
                CurrentPlayer = currentPlayer,
                CurrentTurnIndex = _currentTurn,
                GameStarted = _gameStarted,
                ContagemIniciada = _contagemIniciada,
                TotalTurns = _gameStarted ? (_currentTurn + 1) : 0,
                Status = statusMessage
            };
        }

        public void ResetGame()
        {
            _gameStarted = false;
            _currentTurn = 0;
            _turnOrder.Clear();
            _contagemIniciada = false;
            _gameStartTime = DateTime.MinValue;
            _lastActionTime = DateTime.MinValue;
        }
    }
}
