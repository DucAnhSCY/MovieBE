using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBookingTIcket.Models2;

[Table("Show")]
public partial class Show
{
    [Key]
    [Column("Show_ID")]
    [StringLength(10)]
    [Unicode(false)]
    public string ShowId { get; set; } = null!;

    [Column("Show_Time")]
    public TimeOnly ShowTime { get; set; }

    [Column("Show_Date")]
    public DateOnly ShowDate { get; set; }

    [Column("Seats_Remaining_Gold")]
    public int SeatsRemainingGold { get; set; }

    [Column("Seats_Remaining_Silver")]
    public int SeatsRemainingSilver { get; set; }

    [Column("Class_Cost_Gold")]
    public int ClassCostGold { get; set; }

    [Column("Class_Cost_Silver")]
    public int ClassCostSilver { get; set; }

    [Column("Screen_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string ScreenId { get; set; } = null!;

    [Column("Movie_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string MovieId { get; set; } = null!;

    [InverseProperty("Show")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("MovieId")]
    [InverseProperty("Shows")]
    public virtual Movie Movie { get; set; } = null!;

    [ForeignKey("ScreenId")]
    [InverseProperty("Shows")]
    public virtual Screen Screen { get; set; } = null!;
}
