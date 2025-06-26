using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBE.Models2;

[Table("Theatre")]
public partial class Theatre
{
    [Key]
    [Column("Theatre_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string TheatreId { get; set; } = null!;

    [Column("Name_of_Theatre")]
    [StringLength(30)]
    [Unicode(false)]
    public string NameOfTheatre { get; set; } = null!;

    [Column("No_of_Screens")]
    public int? NoOfScreens { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Area { get; set; }

    [InverseProperty("Theatre")]
    public virtual ICollection<Screen> Screens { get; set; } = new List<Screen>();
}
