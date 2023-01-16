namespace FinalProjectBackend.DTOModel
{
    public class ChallengeDTO
    {
        public int ChallengeId { get; set; }
        public string ChallengeName { get; set; }
        public string ChallengeDescription { get; set; }
        public string ChallengeColor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysLeft { get; set; }   
        public int UserId { get; set; }
    }
}


namespace FinalProjectBackend.DTOModel
{
    public class DailyMsgDTO
    {
        public int DailyMsgId { get; set; }
        public DateTime DateOfWriting { get; set; }
        public string? Description { get; set; }
    }
}

