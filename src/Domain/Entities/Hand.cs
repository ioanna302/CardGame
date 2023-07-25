using ioanna.cardGame.Domain.ValueObjects;
using MoreLinq.Extensions;

namespace ioanna.cardGame.Domain.Entities
{
    public class Hand
    {
        public int HandId { get; set; }
        public List<GameCard> Cards { get;  set; }

        public Hand()
        {
            Cards = new List<GameCard>();
        }

        public void AddCards(IEnumerable<GameCard> cards)
        {
            Cards.AddRange(cards);
            SortHand();
        }

        public void RemoveCard(GameCard card)
        {
            Cards.Remove(card);
        }

        public override string ToString()
        {
            var result = "";
            result += string.Join("\n", Cards.Select((c, index) => $"[{index}] {c}"));
            return result;
        }

        private void SortHand()
        {
            Cards = Cards.OrderBy(c => c.Suit)
                .ThenByDescending(c => c.Rank)
                .ToList();
        }
    }
}