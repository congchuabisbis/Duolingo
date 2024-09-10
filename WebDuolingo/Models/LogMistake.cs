using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class LogMistake
{
    public string IdUser { get; set; } = null!;

    public int IdQues { get; set; }

    public DateTime? DatetimeQues { get; set; }

    public int? ContQues { get; set; }

    public virtual Question IdQuesNavigation { get; set; } = null!;

    public virtual AspNetUser IdUserNavigation { get; set; } = null!;
}
