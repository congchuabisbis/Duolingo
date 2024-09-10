using WebDuolingo.Models;

namespace WebDuolingo.DTO
{
    public class CorrectAnswer
    {
        public Question question { get; set; }
        public Answer answer { get; set; }

        public CorrectAnswer()
        {
            this.question = new Question();
            this.answer = new Answer();
        }
    }
}
