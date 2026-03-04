namespace WebNangCao_MVC_Model.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        // Foreign key
        public int QuestionId { get; set; }

        // Navigation property (nullable)
        public Question? Question { get; set; }
    }
}