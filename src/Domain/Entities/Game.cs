using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;

namespace ioanna.cardGame.Domain.Entities;

public class Game
{
    public List<Player> Players { get; }

    private Trick CurrentTrick { get; set; }

    public List<Trick> PlayedTricks { get; }

    public bool IsFinished { get; private set; }

    public Suit MasterCardSuit { get; private set; }

    public Player CurrentPlayer { get; private set; }
    
    private Deck GameDeck { get; set; }

    public Game(List<Player> players, Deck deck)
    {
        Players = players;
        CurrentTrick = new Trick(Suit.Unknown);
        PlayedTricks = new List<Trick>();
        IsFinished = false;
        CurrentPlayer = players[0];
        GameDeck = deck;
    }

    public void PlayCard(Guid playerId, GameCard card)
    {
        CurrentTrick.AddCard(card);

        if (CurrentTrick.ActiveSuit == Suit.Unknown)
        {
            CurrentTrick.SetActivePip(card.Suit);
        }

        GetPlayerById(playerId).Hand.RemoveCard(card);

        if (CurrentTrick.IsComplete(Players.Count))
        {
            var trickWinner = DetermineTrickWinner();
            CurrentTrick.SetTrickWinner(trickWinner);

            PlayedTricks.Add(CurrentTrick);

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

    public bool CanPlayCard(Guid playerId, GameCard card)
    {
        if (!IsPlayerTurn(playerId))
        {
            return false;
        }

        if (!IsCardInPlayerHand(playerId, card))
        {
            return false;
        }

        if (!IsCardValidForTrick(card, playerId))
        {
            return false;
        }

        return true;
    }

    public void ChangePlayerTurn()
    {
        Console.Clear();
        Console.WriteLine(this);

        var currentPlayerIndex = Players.FindIndex(p => p.Id == CurrentPlayer.Id);

        var nextPlayerIndex = (currentPlayerIndex + 1) % Players.Count;

        CurrentPlayer = Players[nextPlayerIndex];
    }

    public void SetMasterCardPip(Suit suit)
    {
        MasterCardSuit = suit;
    }

    public void DealCardsToAllPlayers(int count)
    {
        Players.ForEach(p => p.Hand.AddCards(
            GameDeck.Draw(count)
                .Select(c => new GameCard(
                    c.Suit,
                    c.Rank,
                    p.Id,
                    c.Suit == MasterCardSuit
                ))));
    }

    private bool IsPlayerTurn(Guid playerId)
    {
        return CurrentPlayer.Id == playerId;
    }

    private bool IsCardInPlayerHand(Guid playerId, Card card)
    {
        var player = GetPlayerById(playerId);

        return player != null && player.Hand.Cards.Contains(card);
    }

    private bool PlayerHasActivePipCards(Player player)
    {
        return player.Hand.Cards.Any(c => c.Suit == CurrentTrick.ActiveSuit);
    }

    private bool PlayerHasHigherMasterCardToPlay(Player player)
    {
        return player.Hand.Cards.Any(c =>
            c.IsMasterCard
            && c.Order > CurrentTrick.HighestMasterPipCard()?.Order);
    }

    private bool PlayerHasMasterCards(Player player)
    {
        return player.Hand.Cards.Any(c => c.Suit == MasterCardSuit);
    }

    private bool IsCardValidForTrick(GameCard card, Guid playerId)
    {
        var player = GetPlayerById(playerId);

        return IsFirstTrickCard() ||
               IsActiveSuitCard(card) ||
               IsValidMasterCard(card, player) ||
               IsValidInactiveCard(player);
    }

    private bool IsFirstTrickCard()
    {
        return CurrentTrick.Cards.Count == 0;
    }

    private bool IsValidInactiveCard(Player player)
    {
        return (!PlayerHasMasterCards(player) && !PlayerHasActivePipCards(player));
    }

    private bool IsValidMasterCard(GameCard card, Player player)
    {
        return (!PlayerHasActivePipCards(player)
                && card.Suit == MasterCardSuit
                && CardIsHighestMasterCard(card) ||
                !PlayerHasHigherMasterCardToPlay(
                    player)
            );
    }

    private bool CardIsHighestMasterCard(GameCard card)
    {
        return (CurrentTrick.HighestMasterPipCard() == null ||
                card.Order > CurrentTrick.HighestMasterPipCard()?.Order);
    }

    private bool IsActiveSuitCard(GameCard card)
    {
        return card.Suit == CurrentTrick.ActiveSuit;
    }

    private Player GetPlayerById(Guid playerId)
    {
        return Players.Find(p => p.Id == playerId);
    }

    private Player DetermineTrickWinner()
    {
        var masterCards = GetTrickMasterCards();

        if (masterCards.Any(m => m.Rank == Rank.Jack))
        {
            var playerId = masterCards.Single(m => m.Rank == Rank.Jack).PlayerId;

            return GetPlayerById(playerId);
        }

        if (masterCards.Any(m => m.Rank == Rank.Nine))
        {
            var playerId = masterCards.Single(m => m.Rank == Rank.Nine).PlayerId;

            return GetPlayerById(playerId);
        }

        var activePipCards = GetTrickActiveCards();

        var candidateCards = masterCards.Count > 0
            ? masterCards
            : activePipCards;

        var highestCard = candidateCards.MaxBy(c => c.Rank);

        Player trickWinner = GetPlayerById(highestCard!.PlayerId);

        return trickWinner;
    }

    private IEnumerable<GameCard> GetTrickActiveCards()
    {
        return CurrentTrick.Cards
            .Where(c => c.Suit == CurrentTrick.ActiveSuit);
    }

    private List<GameCard> GetTrickMasterCards()
    {
        return CurrentTrick.Cards
            .Where(c => c.Suit == MasterCardSuit)
            .ToList();
    }

    private void StartNewTrick(Player trickWinner)
    {
        Console.WriteLine(this);
        // Start a new trick
        CurrentTrick = new Trick(MasterCardSuit);
        // Set the trick winner as the current player for the next trick
        var trickWinnerIndex = Players.FindIndex(p => p.Id == trickWinner.Id);
        CurrentPlayer = Players[trickWinnerIndex];
    }

    public override string ToString()
    {
        var output = "";

        output += $"Master Cards are {MasterCardSuit}";

        CurrentTrick.Cards.ForEach(c =>
        {
            output += $"\n {GetPlayerById(c.PlayerId).Name} played {c}";
        });

        return output;
    }

    private List<(int, int)> CalculateTeamsScores()
    {
        var lastTrickTeam = PlayedTricks.Last().TrickWinner.TeamId;
        var team1TrickScore = CalculateTeamTrickScores(1);
        var team2TrickScore = CalculateTeamTrickScores(2);

        switch (lastTrickTeam)
        {
            case 1:
                team1TrickScore += 10;
                break;
            case 2:
                team2TrickScore += 10;
                break;
        }

        return new List<(int, int)> { (1, team1TrickScore), (2, team2TrickScore) };
    }

    private static int GetTrickCardScore(GameCard card, Suit masterSuit)
    {
        return card.Suit == masterSuit
            ? GetMasterCardScore(card)
            : GetCardScore(card);
    }

    private static int CalculateTrickCardsScore(IEnumerable<GameCard> trickCards, Suit masterSuit)
    {
        return trickCards.Sum(card => GetTrickCardScore(card, masterSuit));
    }

    private int CalculateTeamTrickScores(int teamId)
    {
        var teamPlayers = Players.Where(p => p.TeamId == teamId);
        return teamPlayers.Sum(p =>
        {
            var teamTricks = PlayedTricks
                .Where(t => t.TrickWinner == p)
                .SelectMany(t => t.Cards)
                .ToList();

            return CalculateTrickCardsScore(teamTricks, MasterCardSuit);
        });
    }

    private static int GetCardScore(GameCard card)
    {
        return card.IsMasterCard
            ? GetMasterCardScore(card)
            : GetRegularCardScore(card);
    }

    private static int GetRegularCardScore(GameCard card)
    {
        return card.Rank switch
        {
            Rank.Seven => 0,
            Rank.Eight => 0,
            Rank.Nine => 0,
            Rank.Ten => 10,
            Rank.Jack => 2,
            Rank.Queen => 3,
            Rank.King => 4,
            Rank.Ace => 11,
            Rank.Unknown => throw new ArgumentOutOfRangeException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static int GetMasterCardScore(GameCard card)
    {
        return card.Rank switch
        {
            Rank.Seven => 0,
            Rank.Eight => 0,
            Rank.Nine => 14,
            Rank.Ten => 10,
            Rank.Jack => 22,
            Rank.Queen => 3,
            Rank.King => 4,
            Rank.Ace => 11,
            Rank.Unknown => throw new ArgumentOutOfRangeException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public List<(int, int)> GetGameScores()
    {
        if (!IsFinished)
        {
            throw new InvalidOperationException("The game is not yet finished.");
        }

        return CalculateTeamsScores();
    }
}