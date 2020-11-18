using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;

namespace MyBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MyBlog.Models.Blog> Blog { get; set; }
        public DbSet<MyBlog.Models.Post> Post { get; set; }
        public DbSet<MyBlog.Models.Comment> Comment { get; set; }
        public DbSet<MyBlog.Models.Tag> Tag { get; set; }
        public DbSet<Like> Like { get; set; }
    }
}
