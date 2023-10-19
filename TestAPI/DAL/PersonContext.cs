using Microsoft.EntityFrameworkCore;
using TestAPI.Model;

namespace TestAPI.DAL
{
    public class PersonContext : DbContext{

        public PersonContext(DbContextOptions<PersonContext> options) : base(options) {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.UseIdentityColumns();
            modelBuilder.Entity<Person>().Property(person => person.Id).HasIdentityOptions(startValue: 100);

            modelBuilder.Entity<Person>().HasData(
                new Person(1,"MikieD","Mike","Dragert","blue",2,0),
                new Person(2,"James","James","Young","blue",2,0),
                new Person(3,"Rossco","Ross","Struthers","green",0,0)
            );
        }
    }
}
