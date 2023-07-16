using CSharpFunctionalExtensions;
using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace ioanna.cardGame.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; private set; }
        
        public List<Player> Players { get; set; }
        
        public Trick CurrentTrick { get; private set; }
        
        public List<Trick> PlayedTricks { get;  }
        
        public bool IsFinished { get; private set; }
        
        public Pip MasterCardPip { get; private set; }
        
        public Player CurrentPlayer { get; private set; }
        
        public Game()
        {
            Players = new List<Player>();
        }

        public Game(List<Player> players)
        {
            Id = Guid.NewGuid();
            Players = players;
            CurrentTrick = new Trick(Pip.Unknown);
            PlayedTricks = new List<Trick>();
            IsFinished = false;
            CurrentPlayer = players[0];
        }

        public void PlayCard(Guid playerId, Card card)
        {
            // Validate that it is the player's turn to play
            CanPlayCard(playerId, card);

            // Play the card in the current trick
            var trickCard = new TrickCard(card.Pip, card.PipValue, playerId);
            
            CurrentTrick.AddCard(trickCard);

            if (CurrentTrick.ActivePip == Pip.Unknown)
            {
                CurrentTrick.SetActivePip(trickCard.Pip);
            }

            // Remove the card from the player's hand
            GetPlayerById(playerId).Hand.RemoveCard(card);

            // Check if the trick is complete
            if (CurrentTrick.IsComplete(Players.Count))
            {
                // Determine the winner of the trick
                var trickWinner = DetermineTrickWinner();
                CurrentTrick.SetTrickWinner(trickWinner);

                // Update game state based on trick winner, scores, etc.
                PlayedTricks.Add(CurrentTrick);

                // Start a new trick
                if (Players.Max(p => p.Hand.Cards.Count) == 0)
                {
                    IsFinished = true;
                }
                else
                {
                    StartNewTrick(trickWinner);
                }
            }
            else
            {
                ChangePlayerTurn();
            }
        }

        public Result CanPlayCard(Guid playerId, Card card)
        {
            if (!IsPlayerTurn(playerId))
            {
                return Result.Failure("It is not this player's turn");
            }

            // Validate that the player has the card in their hand
            if (!IsCardInPlayerHand(playerId, card))
            {
                return Result.Failure("Card is not in player's hand");
            }

            if (!IsCardValidForTrick(card, playerId))
            {
                return Result.Failure("Card is not valid for this trick");
            }
            
            return Result.Success();
        }

        public void ChangePlayerTurn()
        {
            Console.Clear();
            Console.WriteLine(this);
            
            var currentPlayerIndex = Players.FindIndex(p => p.Id == CurrentPlayer.Id);

            var nextPlayerIndex = (currentPlayerIndex + 1) % Players.Count;

            CurrentPlayer = Players[nextPlayerIndex];

        }

        public void SetMasterCardPip(Pip pip)
        {
            MasterCardPip = pip;
        }

        private bool IsPlayerTurn(Guid playerId)
        {
            // Implement your own logic to determine if it is the player's turn
            // based on the game rules, current player index, etc.
            // Return true if it is the player's turn; otherwise, return false.

            // Example implementation:
            return CurrentPlayer.Id == playerId;
        }

        private bool IsCardInPlayerHand(Guid playerId, Card card)
        {
            var player = GetPlayerById(playerId);

            return player != null && player.Hand.Cards.Contains(card);
        }

        private bool PlayerHasActivePipCards(Player player)
        {
            return player.Hand.Cards.Any(c => c.Pip == CurrentTrick.ActivePip);
        }

        private bool PlayerHasMasterCards(Player player)
        {
            return player.Hand.Cards.Any(c => c.Pip == MasterCardPip);
        }

        private bool IsCardValidForTrick(Card card, Guid playerId)
        {
            var player = GetPlayerById(playerId);
            
            return CurrentTrick.Cards.Count == 0 ||
                   card.Pip == CurrentTrick.ActivePip ||
                   (!PlayerHasActivePipCards(player) && card.Pip == MasterCardPip &&
                    (CurrentTrick.HighestMasterPipCard() == null ||
                    card.PipValue > CurrentTrick.HighestMasterPipCard()?.PipValue) )||
                   (!PlayerHasMasterCards(player) && !PlayerHasActivePipCards(player));
        }

        private Player GetPlayerById(Guid playerId)
        {
            return Players.Find(p => p.Id == playerId);
        }

        private Player DetermineTrickWinner()
        {
            var masterCards = GetTrickMasterCards();

            if (masterCards.Any(m => m.PipValue == PipValue.Jack))
            {
                var playerId = masterCards.Single(m => m.PipValue == PipValue.Jack).PlayerId;

                return GetPlayerById(playerId);
            }

            if (masterCards.Any(m => m.PipValue == PipValue.Nine))
            {
                var playerId = masterCards.Single(m => m.PipValue == PipValue.Nine).PlayerId;

                return GetPlayerById(playerId);
            }
            
            var activePipCards = GetTrickActiveCards();

            var candidateCards = masterCards.Count > 0
                ? masterCards
                : activePipCards;

            var highestCard = candidateCards.MaxBy(c => c.PipValue);
            
            Player trickWinner = GetPlayerById(highestCard!.PlayerId);

            return trickWinner;
        }

        private IEnumerable<TrickCard> GetTrickActiveCards()
        {
            return CurrentTrick.Cards
                .Where(c => c.Pip == CurrentTrick.ActivePip);
        }

        private List<TrickCard> GetTrickMasterCards()
        {
            return CurrentTrick.Cards
                .Where(c => c.Pip == MasterCardPip)
                .ToList();
        }

        private void StartNewTrick(Player trickWinner)
        {
            Console.WriteLine(this);
            // Start a new trick
            CurrentTrick = new Trick(MasterCardPip);
            // Set the trick winner as the current player for the next trick
            var trickWinnerIndex = Players.FindIndex(p => p.Id == trickWinner.Id);
            CurrentPlayer = Players[trickWinnerIndex];
        }

        public override string ToString()
        {
            var output = "";

            output += $"Master Cards are {MasterCardPip}";
            
            CurrentTrick.Cards.ForEach(c =>
            {
                output += $"\n {GetPlayerById(c.PlayerId).Name} played {c}";
            });

            return output;
        }
    }
}