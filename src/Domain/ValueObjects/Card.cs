using ioanna.cardGame.Domain.Enums;

namespace ioanna.cardGame.Domain.ValueObjects;

public class Card
    
{
    public Card()
    {
    }
    
    public int CardId { get; set; }

    public Suit Suit { get; } 

    public Rank Rank { get; }
    
    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
    
    