using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class QuestionType
{
    public int IdType { get; set; }

    public string? Name { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
