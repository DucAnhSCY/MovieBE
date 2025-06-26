using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBE.Models2;

[Table("Web_user")]
[Index("EmailId", Name = "IX_Web_user_Email")]
[Index("RoleId", Name = "IX_Web_user_Role")]
[Index("EmailId", Name = "UQ__Web_user__B795559E4DC53EF8", IsUnique = true)]
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
    [StringLength(50)]
    [Unicode(false)]
    public string EmailId { get; set; } = null!;

    [Column("Password_Hash")]
    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    public int? Age { get; set; }

    [Column("Phone_Number")]
    [StringLength(15)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [Column("Role_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? RoleId { get; set; }

    [Column("Is_Active")]
    public bool? IsActive { get; set; }

    [Column("Created_Date", TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [Column("Last_Login", TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("RoleId")]
    [InverseProperty("WebUsers")]
    public virtual UserRole? Role { get; set; }
}
