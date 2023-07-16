using CommunityToolkit.Diagnostics;

namespace ioanna.cardGame.Domain.Entities;

public class Team
{
    public string Name { get; }
    
    public List<Player> Members { get; }
    
    public int Score { get; private set; }

    public Team(string name)
    {
        Guard.IsNotNullOrWhiteSpace(name);

        Name = name;
        
        Members = new List<Player>();

        Score = 0;
    }

    public void AddMember(Player player)
    {
        Guard.HasSizeLessThan(Members, 2);
        
        Members.Add(player);
    }

    public void AddToScore(int matchScore)
    {
        Guard.IsGreaterThanOrEqualTo(matchScore, 0);
        
        Score += matchScore;
    }
}