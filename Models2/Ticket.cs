﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieBE.Models2;

[Table("Ticket")]
public partial class Ticket
{
    [Key]
    [Column("Ticket_ID")]
    [StringLength(20)]
    [Unicode(false)]
    public string TicketId { get; set; } = null!;

    [Column("Booking_ID")]
    [StringLength(10)]
    [Unicode(false)]
    public string? BookingId { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string Class { get; set; } = null!;

    public int Price { get; set; }

    [Column("Seat_Number")]
    [StringLength(10)]
    [Unicode(false)]
    public string? SeatNumber { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("Tickets")]
    public virtual Booking? Booking { get; set; }
}
