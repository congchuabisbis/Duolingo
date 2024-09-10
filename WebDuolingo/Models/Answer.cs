using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class Answer
{
    public int IdAns { get; set; }

    public string? NameAns { get; set; }

    public int? IdQues { get; set; }

    public string? ImageAns { get; set; }

    public virtual Question? IdQuesNavigation { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
