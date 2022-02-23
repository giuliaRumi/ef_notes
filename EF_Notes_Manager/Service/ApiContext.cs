using System;
using Microsoft.EntityFrameworkCore;

namespace EF_Notes_Manager.Service
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}
