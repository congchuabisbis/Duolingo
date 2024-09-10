using WebDuolingo.Models;

namespace WebDuolingo.DTO
{
    public class CheckAnswer
    {
        public int Correct { get; set; }
        public int InCorrect { get; set; }
        public int Total { get; set; }
        public float Score { get; set; }
        public List<CorrectAnswer> WrongAnswer { get; set; }

        public CheckAnswer() {
            Correct = 0;
            InCorrect = 0;
            Total = 0;
            Score = 0;
            WrongAnswer = new List<CorrectAnswer>();
        }
    }
}
