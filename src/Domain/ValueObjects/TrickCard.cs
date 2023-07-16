using ioanna.cardGame.Domain.Enums;

namespace ioanna.cardGame.Domain.ValueObjects;

public class TrickCard: Card
{
    public TrickCard(Pip pip, PipValue pipValue, Guid playerId) : base(pip, pipValue)
    {
        PlayerId = playerId;
    }
    
    public TrickCard() {}

    public Guid PlayerId { get; }
}