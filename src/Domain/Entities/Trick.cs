using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace ioanna.cardGame.Domain.Entities
{
    public class Trick
    {
        public int TrickId { get; set; }
        public List<TrickCard> Cards { get; private set; }

        public TrickCard HighestMasterPipCard()
        {
            var masterCards = Cards.Where(c => c.Pip == MasterPip).ToList();
            if (!masterCards.Any())
            {
                return null;
            }

            var jackCard = masterCards.SingleOrDefault(c => c.PipValue == PipValue.Jack);
            var nineCard = masterCards.SingleOrDefault(c => c.PipValue == PipValue.Nine);

            return jackCard ?? nineCard ?? masterCards.MaxBy(c => c.PipValue);
        }

        public Pip ActivePip { get; private set; }

        public Pip MasterPip { get; }
        
        public Player TrickWinner { get; private set; }
        
        public Trick(Pip masterPip)
        {
            MasterPip = masterPip;
            
            Cards = new List<TrickCard>();
        }

        public Trick()
        {
            
        }

        public void AddCard(TrickCard card)
        {
            Cards.Add(card);
        }

        public void SetActivePip(Pip pip)
        {
            ActivePip = pip;
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