using System;
using System.Collections.Generic;

namespace WebDuolingo.Models;

public partial class Story
{
    public int IdStr { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Image { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual AspNetUser? CreateByNavigation { get; set; }
}
