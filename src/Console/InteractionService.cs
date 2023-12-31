using ioanna.cardGame.Application.Interfaces;
using ioanna.cardGame.Domain.Entities;
using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace Console;

public class InteractionService : IInteractionService
{
    public Task<Pip> AskForMasterCardPip()
    {
        System.Console.Clear();

        System.Console.WriteLine($"Provide The Master Card Pip");

        var pipValues = Enum.GetValues<Pip>();
    
        foreach (var pipValue in pipValues)
        {
            System.Console.WriteLine($"[{Convert.ToInt32(pipValue)}] {pipValue}");
        }
    
        return Task.FromResult((Pip)Convert.ToInt32(System.Console.ReadLine()));
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

    public Task<Card> AskForPlayerCard(Hand hand)
    {
        System.Console.WriteLine("Enter your action:");
        System.Console.WriteLine(hand.ToString());
        
        int cardIndex = Convert.ToInt32(System.Console.ReadLine());

        return Task.FromResult(hand.Cards[cardIndex]);

    }

    public Task DisplayInvalidCardError(string error)
    {
        System.Console.WriteLine($"Error while picking your card: {error}");

        return Task.CompletedTask;
    }

    public Task DisplayFinishedGameInfo(int winnerTeam, int winnerScore)
    {
        System.Console.WriteLine("Game Over");
        System.Console.WriteLine($"Team {winnerTeam} won with score {winnerScore}");

        return Task.CompletedTask;
    }
}