namespace ioanna.cardGame.Domain.Entities
{
    public class Player
    {
        public Guid Id { get;  set; }
        
        public string Name { get;  set; }
        
        public Hand Hand { get;  set; }
        
        public int TeamId { get;  set; }
        

        public Player(string name, int team)
        {
            Id = Guid.NewGuid();
            Name = name;
            Hand = new Hand();
            TeamId = team;
        }

        public Player()
        {
            
        }

        // Add any additional methods or properties as needed for your game
    }
}