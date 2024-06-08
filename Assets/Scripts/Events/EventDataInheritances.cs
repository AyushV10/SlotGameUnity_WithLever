namespace Events
{
    //function to send the score what user is winning
    public class SendScore : EventData
    {
        public readonly int Score;
        public SendScore(int score):base(EventIdentifiers.SendScore)
        {
               Score = score;
        }
    }
    //Function to summon the bonus game or start the bonus game 
    public class BonusSummon : EventData
    {
        public BonusSummon():base(EventIdentifiers.BonusSummon)
        {
        }
    }
}
