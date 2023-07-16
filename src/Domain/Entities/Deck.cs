using ioanna.cardGame.Domain.Enums;
using ioanna.cardGame.Domain.ValueObjects;
using MoreLinq;

namespace ioanna.cardGame.Domain.Entities;

public class Deck
{
    public IList<Card> Cards { get; private set; }

    public void Shuffle()
    {
        Cards = Cards.Shuffle().ToList();
    }

    public List<Card> Draw(int count)
    {
        var result = Cards.Take(count);
       
        Cards = Cards.Skip(count).ToList();

        return result.ToList();
    }

    public Deck()
    {
        Cards = new[]
        {
            new Card(Pip.Spades, PipValue.Seven), new Card(Pip.Spades, PipValue.Eight),
            new Card(Pip.Spades, PipValue.Nine), new Card(Pip.Spades, PipValue.Ten),
            new Card(Pip.Spades, PipValue.Jack), new Card(Pip.Spades, PipValue.Queen),
            new Card(Pip.Spades, PipValue.King), new Card(Pip.Spades, PipValue.Ace),
            new Card(Pip.Clubs, PipValue.Seven), new Card(Pip.Clubs, PipValue.Eight),
            new Card(Pip.Clubs, PipValue.Nine), new Card(Pip.Clubs, PipValue.Ten), new Card(Pip.Clubs, PipValue.Jack),
            new Card(Pip.Clubs, PipValue.Queen), new Card(Pip.Clubs, PipValue.King), new Card(Pip.Clubs, PipValue.Ace),
            new Card(Pip.Diamonds, PipValue.Seven), new Card(Pip.Diamonds, PipValue.Eight),
            new Card(Pip.Diamonds, PipValue.Nine), new Card(Pip.Diamonds, PipValue.Ten),
            new Card(Pip.Diamonds, PipValue.Jack), new Card(Pip.Diamonds, PipValue.Queen),
            new Card(Pip.Diamonds, PipValue.King), new Card(Pip.Diamonds, PipValue.Ace),
            new Card(Pip.Hearts, PipValue.Seven), new Card(Pip.Hearts, PipValue.Eight),
            new Card(Pip.Hearts, PipValue.Nine), new Card(Pip.Hearts, PipValue.Ten),
            new Card(Pip.Hearts, PipValue.Jack), new Card(Pip.Hearts, PipValue.Queen),
            new Card(Pip.Hearts, PipValue.King), new Card(Pip.Hearts, PipValue.Ace)
        };
        
        Shuffle();
    }

}