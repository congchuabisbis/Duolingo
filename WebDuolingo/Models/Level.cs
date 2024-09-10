using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class Level
{
    public int IdLevel { get; set; }

    public string? NameLevel { get; set; }

    public string? NoteLevel { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
