using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class Listen
{
    public int IdListen { get; set; }

    public string? FileListen { get; set; }

    public int? IdQues { get; set; }

    public virtual Question? IdQuesNavigation { get; set; }
}
