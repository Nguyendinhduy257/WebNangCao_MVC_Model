namespace WebNangCao_MVC_Model.Models
{
    public class ExamResultDetail
    {
        public int Id { get; set; }
        public int ExamResultId { get; set; } // Link tới bảng ExamResult bạn vừa khoe
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; } // ID đáp án sinh viên đã chọn

        // Navigation properties
        public virtual ExamResult ExamResult { get; set; }
    }
}
