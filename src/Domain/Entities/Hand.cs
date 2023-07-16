using ioanna.cardGame.Domain.ValueObjects;
using MoreLinq.Extensions;

namespace ioanna.cardGame.Domain.Entities
{
    public class Hand
    {
        public int HandId { get; set; }
        public List<Card> Cards { get;  set; }

        public Hand()
        {
            Cards = new List<Card>();
        }

        public void AddCards(IEnumerable<Card> cards)
        {
            Cards.AddRange(cards);
            SortHand();
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public override string ToString()
        {
            var result = "";
            result += string.Join("\n", Cards.Select((c, index) => $"[{index}] {c}"));
            return result;
        }

        public void SortHand()
        {
            Cards = Cards.OrderBy(c => c.Pip)
                .ThenBy(c => c.PipValue)
                .ToList();
        }

        // Add any additional methods or properties as needed for your game
    }
}