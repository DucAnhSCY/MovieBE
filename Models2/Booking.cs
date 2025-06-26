using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBE.Models2;

[Table("Booking")]
[Index("ShowId", Name = "IX_Booking_Show")]
[Index("UserId", Name = "IX_Booking_User")]
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
    [StringLength(50)]
    [Unicode(false)]
    public string? NameOnCard { get; set; }

    [Column("Booking_Date", TypeName = "datetime")]
    public DateTime? BookingDate { get; set; }

    [Column("Booking_Status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? BookingStatus { get; set; }

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
