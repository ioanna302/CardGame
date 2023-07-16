using ioanna.cardGame.Domain.Entities;
using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace ioanna.cardGame.Application.Interfaces;

public interface IInteractionService
{
    public Task<Pip> AskForMasterCardPip();
    
    public Task DisplayCurrentTurnInfo(Game game);

    public Task<Card> AskForPlayerCard(Hand hand);

    public Task DisplayInvalidCardError(string error);
    
    public Task DisplayFinishedGameInfo(List<(int,int)> scores);
}