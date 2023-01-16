namespace FinalProjectBackend.Model
{
    public class DailyMsg
    {
        public int DailyMsgId { get; set; }
        public DateTime DateOfWriting   { get; set; }
        public string? Description { get; set; }
        public int ChallengeId { get; set; }
        public Challenge? Challenge { get; set; }


    }
}
