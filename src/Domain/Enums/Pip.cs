using System.ComponentModel;

namespace ioanna.cardGame.Domain.Enums;

public enum Pip
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