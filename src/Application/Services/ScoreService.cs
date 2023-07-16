using ioanna.cardGame.Application.Interfaces;
using ioanna.cardGame.Domain.Entities;
using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace ioanna.cardGame.Application.Services
{
    public class ScoreService : IScoreService
    {
        private int CalculateTrickCardsScore(List<TrickCard> trickCards, Pip masterPip)
        {
            return trickCards.Sum(card => GetTrickCardScore(card, masterPip));
        }

        public List<(int, int)> CalculateTeamsScores(Game game)
        {
            var lastTrickTeam = game.PlayedTricks.Last().TrickWinner.TeamId;
            var team1TrickScore = CalculateTeamTrickScores(game, 1);
            var team2TrickScore = CalculateTeamTrickScores(game, 2);

            if (lastTrickTeam == 1)
            {
                team1TrickScore += 10;
            }
            else if (lastTrickTeam == 2)
            {
                team2TrickScore += 10;
            }

            return new List<(int, int)> { (1, team1TrickScore), (2, team2TrickScore) };
        }

        private int GetTrickCardScore(TrickCard card, Pip masterPip)
        {
            return card.Pip == masterPip
                ? GetMasterCardScore(card)
                : GetCardScore(card);
        }
        
        
        private int CalculateTeamTrickScores(Game game, int teamId)
        {

            var teamPlayers = game.Players.Where(p => p.TeamId == teamId);
            return teamPlayers.Sum(p =>
            {
                var teamTricks = game.PlayedTricks
                    .Where(t => t.TrickWinner == p)
                    .SelectMany(t => t.Cards)
                    .ToList();
                
                return CalculateTrickCardsScore(teamTricks, game.MasterCardPip);
            });
        }
        
        private static int GetCardScore(Card card)
        {
            return card.PipValue switch
            {
                PipValue.Seven => 0,
                PipValue.Eight => 0,
                PipValue.Nine => 0,
                PipValue.Ten => 10,
                PipValue.Jack => 2,
                PipValue.Queen => 3,
                PipValue.King => 4,
                PipValue.Ace => 11,
                PipValue.Unknown => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static int GetMasterCardScore(Card card)
        {
            return card.PipValue switch
            {
                PipValue.Seven => 0,
                PipValue.Eight => 0,
                PipValue.Nine => 14,
                PipValue.Ten => 10,
                PipValue.Jack => 22,
                PipValue.Queen => 3,
                PipValue.King => 4,
                PipValue.Ace => 11,
                PipValue.Unknown => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}