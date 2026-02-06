namespace LearningWebsite.Models
{
    public class AssessmentResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LearningId { get; set; }
        public DateTime CompletedDate { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal Score { get; set; } // Percentage
        public bool Passed { get; set; } // Pass if Score >= 70

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Learning? Learning { get; set; }
    }
}
