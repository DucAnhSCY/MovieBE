using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBookingTIcket.Models2;

[Table("Screen")]
public partial class Screen
{
    [Key]
    [Column("Screen_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string ScreenId { get; set; } = null!;

    [Column("No_of_Seats_Gold")]
    public int NoOfSeatsGold { get; set; }

    [Column("No_of_Seats_Silver")]
    public int NoOfSeatsSilver { get; set; }

    [Column("Theatre_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? TheatreId { get; set; }

    [InverseProperty("Screen")]
    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();

    [ForeignKey("TheatreId")]
    [InverseProperty("Screens")]
    public virtual Theatre? Theatre { get; set; }
}
