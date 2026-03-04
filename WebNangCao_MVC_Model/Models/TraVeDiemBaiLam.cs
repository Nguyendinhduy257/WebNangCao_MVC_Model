namespace WebNangCao_MVC_Model.Models
{
    //file này dùng để nhận dữ liệu từ client gửi lên khi submit bài thi, sau đó TestAttemptController.cs sẽ xử lý tính điểm và trả về kết quả
    public class SubmitExamModel
    {
        public int ExamId { get; set; }
        public List<UserAnswerModel> UserAnswers { get; set; }
    }

    public class UserAnswerModel
    {
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
    }
}