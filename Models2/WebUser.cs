using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBookingTIcket.Models2;

[Table("Web_user")]
public partial class WebUser
{
    [Key]
    [Column("Web_User_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string WebUserId { get; set; } = null!;

    [Column("First_Name")]
    [StringLength(15)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [Column("Last_Name")]
    [StringLength(20)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [Column("Email_ID")]
    [StringLength(30)]
    [Unicode(false)]
    public string? EmailId { get; set; }

    public int? Age { get; set; }

    [Column("Phone_Number")]
    [StringLength(10)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
