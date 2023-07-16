using ioanna.cardGame.Domain.Enums;

namespace ioanna.cardGame.Domain.ValueObjects;

public class Card
    
{
    public int CardId { get; set; }
    public Card(Pip pip, PipValue pipValue)
    {
        Pip = pip;
        PipValue = pipValue;
    }

    protected Card()
    {
    }

    public Pip Pip { get; } 

    public PipValue PipValue { get; } 

    public override string ToString()
    {
        return $"{PipValue} of {Pip}";
    }
}
    
    