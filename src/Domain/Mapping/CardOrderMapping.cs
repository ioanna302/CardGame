using ioanna.cardGame.Domain.Enums;

namespace ioanna.cardGame.Domain.Mapping;

public static class CardOrderMapping
{
    public static readonly Dictionary<Rank, int> RegularCardOrder = new()
    {
        { Rank.Seven, 1 },
        { Rank.Eight, 2 },
        { Rank.Queen, 3 },
        { Rank.King, 4 },
        { Rank.Ten, 5 },
        { Rank.Ace, 6 },
        { Rank.Nine, 7 },
        { Rank.Jack, 8 },
    };
    
    public static readonly Dictionary<Rank, int> MasterCardOrder = new()
    {
        { Rank.Seven, 1 },
        { Rank.Eight, 2 },
        { Rank.Nine, 3 },
        { Rank.Jack, 4 },
        { Rank.Queen, 5 },
        { Rank.King, 6 },
        { Rank.Ten, 7 },
        { Rank.Ace, 8 }
    };
}