using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.Mapping;

namespace ioanna.cardGame.Domain.ValueObjects;

public class GameCard: Card
{
    public bool IsMasterCard { get; }

    public Guid PlayerId { get; }
    
    public GameCard(Suit suit,
        Rank rank,
        Guid playerId,
        bool isMasterCard) : base(suit,
        rank)
    {
        PlayerId = playerId;
        IsMasterCard = isMasterCard;
    }
    
    public int Order => IsMasterCard
        ? CardOrderMapping.MasterCardOrder[Rank]
        : CardOrderMapping.RegularCardOrder[Rank];
}