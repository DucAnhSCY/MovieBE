using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieBE.Models2;

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

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<WebUser> WebUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DUCANHLAPTOP;Initial Catalog=MovieBookingTicket;Persist Security Info=True;User ID=sa;Password=2005;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__35ABFDE0F784976A");

            entity.Property(e => e.BookingDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.BookingStatus).HasDefaultValue("Confirmed");

            entity.HasOne(d => d.Show).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Booking__Show_ID__4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Booking__User_ID__4E88ABD4");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movie__7A880405DF54D750");
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.ScreenId).HasName("PK__Screen__1D3FB5CB711CC6A1");

            entity.HasOne(d => d.Theatre).WithMany(p => p.Screens)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Screen__Theatre___4222D4EF");
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(e => e.ShowId).HasName("PK__Show__606BAC1B8A4DAC46");

            entity.HasOne(d => d.Movie).WithMany(p => p.Shows).HasConstraintName("FK__Show__Movie_ID__49C3F6B7");

            entity.HasOne(d => d.Screen).WithMany(p => p.Shows).HasConstraintName("FK__Show__Screen_ID__48CFD27E");
        });

        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.HasKey(e => e.TheatreId).HasName("PK__Theatre__324C3D8DEE64E76B");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__ED7260D90904C395");

            entity.HasOne(d => d.Booking).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ticket__Booking___52593CB8");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__User_Rol__D80AB49B128F4A98");
        });

        modelBuilder.Entity<WebUser>(entity =>
        {
            entity.HasKey(e => e.WebUserId).HasName("PK__Web_user__F08811A7A2A724CC");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleId).HasDefaultValue("R002");

            entity.HasOne(d => d.Role).WithMany(p => p.WebUsers).HasConstraintName("FK__Web_user__Role_I__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
