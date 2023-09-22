
namespace BattleShipServerConsole.Classes
{
    public class Move
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

        public Move()
        {

        }

        public Move( string description, string date)
        {

            Description = description;
            Date = date;
        }
    }
}
