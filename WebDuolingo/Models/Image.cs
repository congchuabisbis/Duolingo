using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class Image
{
    public int? IdImg { get; set; }

    public string? FileImg { get; set; }

    public int? IdQues { get; set; }

    public virtual Question? IdQuesNavigation { get; set; }
}
