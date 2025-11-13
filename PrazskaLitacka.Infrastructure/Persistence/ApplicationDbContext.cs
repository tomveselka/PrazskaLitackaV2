using Microsoft.EntityFrameworkCore;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Race> Races => Set<Race>();
        public DbSet<RaceEntry> RaceEntries => Set<RaceEntry>();
        public DbSet<Row> Rows => Set<Row>();
        public DbSet<BonusLine> BonusLines => Set<BonusLine>();
        public DbSet<BonusStation> BonusStations => Set<BonusStation>();
        public DbSet<Points> Points => Set<Points>();
        public DbSet<Station> Stations => Set<Station>();
        public DbSet<StationLine> StationLines => Set<StationLine>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TechnicalVariables> TechnicalVariables => Set<TechnicalVariables>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Race>()
                .HasMany(r => r.RaceEntries)
                .WithOne(e => e.Race)
                .HasForeignKey(e => e.RaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Race>()
                .HasMany(r => r.BonusStations)
                .WithOne(b => b.Race)
                .HasForeignKey(b => b.RaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Race>()
                .HasMany(r => r.BonusLines)
                .WithOne(l => l.Race)
                .HasForeignKey(l => l.RaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RaceEntry>()
                .HasMany(e => e.Rows)
                .WithOne(r => r.RaceEntry)
                .HasForeignKey(r => r.RaceEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Points seeding
            var seed = new (int id, string name, int pts)[] {
                (1,"Bike",1),
                (2,"Metro",2),
                (3,"BusPrague",3),
                (4,"Tram",4),
                (5,"BusRegional",5),
                (6,"TrainOs",7),
                (7,"TrainR",8),
                (8,"Ferry",9),
                (9,"Trolleybus",6),
                (10,"Funicular",10),
                (11,"STOP",3),
                (12,"BONUS_STOP",20),
                (13,"BONUS_LINE",15),
                (14,"ZONE",25),
                (15,"LATE",10)
            };
            modelBuilder.Entity<Points>().HasData(seed.Select(s => new Points { Id = s.id, Name = s.name, PointsValue = s.pts }));
        }
    }
}
