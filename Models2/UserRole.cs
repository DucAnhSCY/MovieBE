using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBE.Models2;

[Table("User_Role")]
public partial class UserRole
{
    [Key]
    [Column("Role_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string RoleId { get; set; } = null!;

    [Column("Role_Name")]
    [StringLength(20)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<WebUser> WebUsers { get; set; } = new List<WebUser>();
}
