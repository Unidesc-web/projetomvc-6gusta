using Microsoft.EntityFrameworkCore;

namespace projetomvc_6gusta.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
    }
}
