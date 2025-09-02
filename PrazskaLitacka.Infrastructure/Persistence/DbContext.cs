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
                (1,"METRO",2),
                (2,"BUS_CITY",3),
                (3,"TRAM",4),
                (4,"BUS_300",5),
                (5,"BUS_400",6),
                (6,"TRAIN_OS",7),
                (7,"TRAIN_R",8),
                (8,"FERRY",9),
                (9,"TROLLEYBUS",3),
                (10,"FUNICULAR",10),
                (11,"STATION",3),
                (12,"BONUS_STATION",25),
                (13,"BONUS_LINE",25),
                (14,"AREA",20)
            };
            modelBuilder.Entity<Points>().HasData(seed.Select(s => new Points { Id = s.id, Name = s.name, PointsValue = s.pts }));
        }
    }
}
