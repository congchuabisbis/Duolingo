using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class Question
{
    public int IdQues { get; set; }

    public string? NameQues { get; set; }

    public int? Level { get; set; }

    public int? CorrectAns { get; set; }

    public int? IdType { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Answer? CorrectAnsNavigation { get; set; }

    public virtual AspNetUser? CreateByNavigation { get; set; }

    public virtual QuestionType? IdTypeNavigation { get; set; }

    public virtual Level? LevelNavigation { get; set; }

    public virtual ICollection<Listen> Listens { get; set; } = new List<Listen>();

    public virtual ICollection<LogMistake> LogMistakes { get; set; } = new List<LogMistake>();
}
