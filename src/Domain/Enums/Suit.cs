using System.ComponentModel;

namespace ioanna.cardGame.Domain.Enums;

public enum Suit
{
    Unknown = 0,
    
    [Description("♣")]
    Spades,
    
    [Description("♠")]
    Clubs,
    
    [Description("♥")]
    Hearts,
    
    [Description("♦")]
    Diamonds
    
}