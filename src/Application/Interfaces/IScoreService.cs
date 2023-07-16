using ioanna.cardGame.Domain.Entities;

namespace ioanna.cardGame.Application.Interfaces;

public interface IScoreService
{
    public List<(int, int)> CalculateTeamsScores(Game game);

}