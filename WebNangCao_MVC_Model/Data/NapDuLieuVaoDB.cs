using WebNangCao_MVC_Model.Models;
using System.Linq;
using System.Collections.Generic;
namespace WebNangCao_MVC_Model.Data
{
    public class NapDuLieuVaoDB
    {
        public static void Seed(AppDbContext context)
        {
            //kiểm tra nếu đã có dữ liệu chưa? nếu có rồi thì thoát không cần thêm nữa
            if (context.Exams.Any()) return;
            //1. tạo bài thi mẫu
            var exam = new Exam
            {
                Title = "Bài kiểm tra Tiếng Anh B2",
                Duration = 45,
                IsActive = true,
                Questions=new List <Question>()
            };
            //2. tạo danh sách câu hỏi và đáp án
            var q1 = new Question
            {
                Content = "What does 'MVC' stand for in web development?",
                Difficulty="Trung bình",
                Answers=new List<Answer>()
                {
                    new Answer { Content = "Model View Controller", IsCorrect = true },
                    new Answer { Content = "Main Video Center", IsCorrect = false },
                    new Answer { Content = "Modern View Concept", IsCorrect = false }
                }
            };
            var q2 = new Question
            {
                Content = "Which language is primarily used for ASP.NET Core?",
                Difficulty = "Dễ",
                Answers = new List<Answer>
                {
                    new Answer { Content = "Python", IsCorrect = false },
                    new Answer { Content = "C#", IsCorrect = true },
                    new Answer { Content = "Java", IsCorrect = false }
                }
            };
            var q3 = new Question
            {
                Content = "What is the purpose of Entity Framework in ASP.NET Core?",
                Difficulty = "Khó",
                Answers = new List<Answer>()
                {
                    new Answer { Content = "To manage database connections and queries", IsCorrect = true },
                    new Answer { Content = "To handle user authentication", IsCorrect = false },
                    new Answer { Content = "To create user interfaces", IsCorrect = false },
                    new Answer { Content = "To optimize application performance", IsCorrect = false }
                }
            };
            //3 kết nối chúng lại
            exam.Questions.Add(q1);
            exam.Questions.Add(q2);
            exam.Questions.Add(q3);
            //4. lưu vào DB
            context.Exams.Add(exam);
            context.SaveChanges();
        }
    }
}
