using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace ioanna.cardGame.Domain.Entities
{
    public class Trick
    {
        public int TrickId { get; set; }
        public List<GameCard> Cards { get; private set; }

        public GameCard HighestMasterPipCard()
        {
            var masterCards = Cards.Where(c => c.Suit == MasterSuit).ToList();
            if (!masterCards.Any())
            {
                return null;
            }

            return masterCards.MaxBy(c => c.Order);
        }

        public Suit ActiveSuit { get; private set; }

        public Suit MasterSuit { get; }
        
        public Player TrickWinner { get; private set; }
        
        public Trick(Suit masterSuit)
        {
            MasterSuit = masterSuit;
            
            Cards = new List<GameCard>();
        }

        public Trick()
        {
            
        }

        public void AddCard(GameCard card)
        {
            Cards.Add(card);
        }

        public void SetActivePip(Suit suit)
        {
            ActiveSuit = suit;
        }

        public void SetTrickWinner(Player player)
        {
            TrickWinner = player;
        }

        public bool IsComplete(int numberOfPlayers)
        {
            // Check if the trick is complete based on the number of cards played
            return Cards.Count == numberOfPlayers;
        }
    }
}