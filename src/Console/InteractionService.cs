using ioanna.cardGame.Application.Interfaces;
using ioanna.cardGame.Domain.Entities;
using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace Console;

public class InteractionService : IInteractionService
{
    public Task<Suit> AskForMasterCardPip()
    {
        System.Console.Clear();

        System.Console.WriteLine($"Provide The Master Card Suit");

        var pipValues = Enum.GetValues<Suit>();
    
        foreach (var pipValue in pipValues)
        {
            System.Console.WriteLine($"[{Convert.ToInt32(pipValue)}] {pipValue}");
        }
    
        return Task.FromResult((Suit)Convert.ToInt32(System.Console.ReadLine()));
    }

    public Task DisplayCurrentTurnInfo(Game game)
    {
        System.Console.Clear();
        System.Console.BackgroundColor = ConsoleColor.DarkBlue;
        System.Console.WriteLine("Current Player: " + game.CurrentPlayer.Name);
        System.Console.WriteLine(game.ToString());
        System.Console.WriteLine();
        System.Console.ResetColor();

        return Task.CompletedTask;
    }

    public Task<GameCard> AskForPlayerCard(Hand hand)
    {
        System.Console.WriteLine("Enter your action:");
        System.Console.WriteLine(hand.ToString());
        
        int cardIndex = Convert.ToInt32(System.Console.ReadLine());

        return Task.FromResult(hand.Cards[cardIndex]);

    }

    public Task DisplayInvalidCardError(string error)
    {
        System.Console.WriteLine($"Error while picking your cardBase: {error}");

        return Task.CompletedTask;
    }

    public Task DisplayFinishedGameInfo(List<(int, int)> scores)
    {
        System.Console.WriteLine("Game Over");
        System.Console.WriteLine($"Team {scores[0].Item1} has score {scores[0].Item2}");
        System.Console.WriteLine($"Team {scores[1].Item1} has score {scores[1].Item2}");

        return Task.CompletedTask;
    }
}