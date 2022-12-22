

using Microsoft.EntityFrameworkCore;
using projetoapi.Models;

namespace projetoapi.Data
{
    public class PrimaryDbContext : DbContext
    {
        public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options) : base(options)
        {
            
        }
        public DbSet<Aluno>? Alunos { get ; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<Aluno>().HasData(
                new List<Aluno>(){
                    new Aluno(1, "Peterson"),
                    new Aluno(2, "Teste")
                }
            );
        }
    }
}