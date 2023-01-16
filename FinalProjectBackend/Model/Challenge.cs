namespace FinalProjectBackend.Model
{
    public class Challenge
    {
        public int ChallengeId { get; set; }
        public string ChallengeName { get; set; }
        public string ChallengeDescription { get; set; }
        public string ChallengeColor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<WeeklyMsg> WeeklyMsg { get; set; }
        public List<DailyMsg> DailyMsg { get; set; }
        public User User  { get; set; }
        public int UserId { get; set; }
      
    }
}
