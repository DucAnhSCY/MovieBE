using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieBookingTIcket.Models2;

public partial class DBContextTest : DbContext
{
    public DBContextTest()
    {
    }

    public DBContextTest(DbContextOptions<DBContextTest> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Screen> Screens { get; set; }

    public virtual DbSet<Show> Shows { get; set; }

    public virtual DbSet<Theatre> Theatres { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<WebUser> WebUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DUCANHLAPTOP;Initial Catalog=MovieBookingTicket;Persist Security Info=True;User ID=sa;Password=2005;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__35ABFDE08724FB35");

            entity.HasOne(d => d.Show).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Booking__Show_ID__46E78A0C");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Booking__User_ID__45F365D3");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movie__7A8804058A429551");
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.ScreenId).HasName("PK__Screen__1D3FB5CB04CC0F64");

            entity.HasOne(d => d.Theatre).WithMany(p => p.Screens)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Screen__Theatre___3B75D760");
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(e => e.ShowId).HasName("PK__Show__606BAC1B3FBCB619");

            entity.HasOne(d => d.Movie).WithMany(p => p.Shows).HasConstraintName("FK__Show__Movie_ID__4316F928");

            entity.HasOne(d => d.Screen).WithMany(p => p.Shows).HasConstraintName("FK__Show__Screen_ID__4222D4EF");
        });

        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.HasKey(e => e.TheatreId).HasName("PK__Theatre__324C3D8D911245FA");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__ED7260D91B1BFD1E");

            entity.HasOne(d => d.Booking).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ticket__Booking___49C3F6B7");
        });

        modelBuilder.Entity<WebUser>(entity =>
        {
            entity.HasKey(e => e.WebUserId).HasName("PK__Web_user__F08811A714CD7DEC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
