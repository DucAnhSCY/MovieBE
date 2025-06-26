using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBookingTIcket.Models2;

[Table("Booking")]
public partial class Booking
{
    [Key]
    [Column("Booking_ID")]
    [StringLength(10)]
    [Unicode(false)]
    public string BookingId { get; set; } = null!;

    [Column("No_of_Tickets")]
    public int NoOfTickets { get; set; }

    [Column("Total_Cost")]
    public int TotalCost { get; set; }

    [Column("Card_Number")]
    [StringLength(19)]
    [Unicode(false)]
    public string? CardNumber { get; set; }

    [Column("Name_on_card")]
    [StringLength(21)]
    [Unicode(false)]
    public string? NameOnCard { get; set; }

    [Column("User_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("Show_ID")]
    [StringLength(10)]
    [Unicode(false)]
    public string? ShowId { get; set; }

    [ForeignKey("ShowId")]
    [InverseProperty("Bookings")]
    public virtual Show? Show { get; set; }

    [InverseProperty("Booking")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual WebUser? User { get; set; }
}
