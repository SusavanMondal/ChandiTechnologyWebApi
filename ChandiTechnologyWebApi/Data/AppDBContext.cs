
using ChandiTechnologyWebApi.Model;
using Microsoft.EntityFrameworkCore;




namespace ChandiTechnologyWebApi.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
       : base(options)
        {
        }


        public DbSet<Agent> Agents { get; set; }
        public DbSet<HotelBooking> HotelBookings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Agent>()
                .HasIndex(a => a.Email)
                .IsUnique();


            modelBuilder.Entity<HotelBooking>()
                .HasOne(b => b.Agent)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.AgentID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<HotelBooking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");



        }


    }
}
