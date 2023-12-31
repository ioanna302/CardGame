using ioanna.cardGame.Application.Interfaces;
using ioanna.cardGame.Domain.Entities;

namespace ioanna.cardGame.Application.Services
{
    public class GameFlowService : IGameFlowService
    {
        private readonly ScoreService _scoreService;
        private readonly IInteractionService _interactionService;

        public GameFlowService(ScoreService scoreService, IInteractionService interactionService)
        {
            _scoreService = scoreService;
            _interactionService = interactionService;
        }

        public async Task StartApplication()
        {
            Game game = InitGame();

            var deck = new Deck();
            
            deck.Shuffle();

            await StartMasterCardPipSelectionRound(game);

            StartExtraPointsRound(game);

            await StartCardPlayingRound(game, deck);

        }

        private static Game InitGame()
        {
            var player1 = new Player("Player 1", 1);
            var player2 = new Player("Player 2", 2);
            var player3 = new Player("Player 3", 1);
            var player4 = new Player("Player 4", 2);

            var game = new Game(new List<Player> { player1, player2, player3, player4 });
            return game;
        }

        private async Task StartCardPlayingRound(Game game, Deck deck)
        {
            game.Players.ForEach(p => p.Hand.AddCards(deck.Draw(8)));
            
            while (!game.IsFinished)
            {
                _interactionService.DisplayCurrentTurnInfo(game);
               
                var playingCard = await _interactionService.AskForPlayerCard(game.CurrentPlayer.Hand);

                var isValidResult = game.CanPlayCard(game.CurrentPlayer.Id, playingCard);
                
                while (isValidResult.IsFailure)
                {
                    await _interactionService.DisplayInvalidCardError(isValidResult.Error);

                    await _interactionService.AskForPlayerCard(game.CurrentPlayer.Hand);
                }
                
                game.PlayCard(game.CurrentPlayer.Id, playingCard);
            }

            var winner = GetGameWinnerTeam(game);
          
            var winnerScore = CalculateTeamTrickScores(game, winner);
            
            await _interactionService.DisplayFinishedGameInfo(winner, winnerScore);

        }

        private void StartExtraPointsRound(Game game)
        {
        }

        private async Task StartMasterCardPipSelectionRound(Game game)
        {
            var masterCardPip = await _interactionService.AskForMasterCardPip();

            game.SetMasterCardPip(masterCardPip);
        }

        private int GetGameWinnerTeam(Game game)
        {
            // Retrieve the game from the repository
            if (!game.IsFinished)
            {
                throw new InvalidOperationException("The game is not yet finished.");
            }
            
            return CalculateTeamTrickScores(game, 0) > CalculateTeamTrickScores(game,1) 
                ? 0 : 1;
        }


        private int CalculateTeamTrickScores(Game game, int teamId)
        {

            var teamPlayers = game.Players.Where(p => p.TeamId == teamId);
            return teamPlayers.Sum(p =>
            {
                var teamTricks = game.PlayedTricks
                    .Where(t => t.TrickWinner == p)
                    .SelectMany(t => t.Cards)
                    .ToList();
                
               return _scoreService.CalculateTrickCardsScore(teamTricks, game.MasterCardPip);
            });
        }
    }
}