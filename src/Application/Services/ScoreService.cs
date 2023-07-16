using ioanna.cardGame.Application.Interfaces;
using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace ioanna.cardGame.Application.Services
{
    public class ScoreService : IScoreService
    {
        public int CalculateTrickCardsScore(List<TrickCard> trickCards, Pip masterPip)
        {
            return trickCards.Sum(card => GetTrickCardScore(card, masterPip));
        }

        public int GetTrickCardScore(TrickCard card, Pip masterPip)
        {
            return card.Pip == masterPip
                ? GetMasterCardScore(card)
                : GetCardScore(card);
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
                PipValue.Jack => 20,
                PipValue.Queen => 3,
                PipValue.King => 4,
                PipValue.Ace => 11,
                PipValue.Unknown => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}