using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBookingTIcket.Models2;

[Table("Movie")]
public partial class Movie
{
    [Key]
    [Column("Movie_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string MovieId { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? Language { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Genre { get; set; }

    [Column("Target_Audience")]
    [StringLength(5)]
    [Unicode(false)]
    public string? TargetAudience { get; set; }

    [InverseProperty("Movie")]
    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
}
