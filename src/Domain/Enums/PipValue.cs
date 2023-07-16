using System.ComponentModel;

namespace ioanna.cardGame.Domain.Enums;

public enum PipValue
{
    Unknown = 0,
    
    [Description("7")]
    Seven = 1,

    [Description("8")]
    Eight = 2,
    
    [Description("9")]
    Nine = 3,
    
    [Description("J")]
    Jack = 4,
    
    [Description("Q")]
    Queen = 5,
    
    [Description("K")]
    King = 6,
    
    [Description("10")]
    Ten = 7,

    [Description("A")]
    Ace = 8
}