namespace FinalProjectBackend.Model
{
    public class WeeklyMsg
    {
        public int WeeklyMsgId { get; set; }
        public DateTime DateOfWriting { get; set; }
        public string WeeklyMsgDescription { get; set; }
        public bool Checked { get; set; }
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; }
    }
}
