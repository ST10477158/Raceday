using Microsoft.EntityFrameworkCore;
using Raceday.Models;
using Results = Raceday.Models.Results;
using Route = Raceday.Models.Route;

namespace Raceday.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Organiser> Organisers { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Results> Results { get; set; }
    }
}
