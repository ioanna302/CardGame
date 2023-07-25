using ioanna.cardGame.Application.Interfaces;
using ioanna.cardGame.Domain.Entities;

namespace ioanna.cardGame.Application.Services;

public class GameFlowService : IGameFlowService
{
    private readonly IInteractionService _interactionService;

    public GameFlowService(IInteractionService interactionService)
    {
        _interactionService = interactionService;
    }

    public async Task StartApplication()
    {
        var deck = new Deck();
            
        deck.Shuffle();
        
        Game game = InitGame(deck);

        await StartMasterCardPipSelectionRound(game);

        StartExtraPointsRound(game);

        await StartCardPlayingRound(game);

    }

    private static Game InitGame(Deck deck)
    {
        var player1 = new Player("Player 1", 1);
        var player2 = new Player("Player 2", 2);
        var player3 = new Player("Player 3", 1);
        var player4 = new Player("Player 4", 2);

        var game = new Game(new List<Player> { player1, player2, player3, player4 }, deck);
        return game;
    }

    private async Task StartCardPlayingRound(Game game)
    {
        game.DealCardsToAllPlayers(8);
            
        while (!game.IsFinished)
        {
            await _interactionService.DisplayCurrentTurnInfo(game);
               
            var playingCard = await _interactionService.AskForPlayerCard(game.CurrentPlayer.Hand);
                
            while (!game.CanPlayCard(game.CurrentPlayer.Id, playingCard))
            {
                await _interactionService.DisplayInvalidCardError("Invalid card");

                playingCard = await _interactionService.AskForPlayerCard(game.CurrentPlayer.Hand);
            }
                
            game.PlayCard(game.CurrentPlayer.Id, playingCard);
        }

        var scores = game.GetGameScores();

        await _interactionService.DisplayFinishedGameInfo(scores);

    }

 

    private void StartExtraPointsRound(Game game)
    {//todo implement
    }

    private async Task StartMasterCardPipSelectionRound(Game game)
    {
        var masterCardPip = await _interactionService.AskForMasterCardPip();

        game.SetMasterCardPip(masterCardPip);
    }


}