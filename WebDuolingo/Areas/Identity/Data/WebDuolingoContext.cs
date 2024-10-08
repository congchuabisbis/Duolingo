﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebDuolingo.Areas.Identity.Data;

namespace WebDuolingo.Data;

public class WebDuolingoContext : IdentityDbContext<WebDuolingoUser>
{
    public WebDuolingoContext(DbContextOptions<WebDuolingoContext> options)
        : base(options)
    {
    }

    public object Questions { get; internal set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
