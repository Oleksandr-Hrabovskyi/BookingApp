using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Contracts.Database;

[Table("tbl_bookings", Schema = "public")]
public class Booking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; init; }

    [Column("first_name")]
    [Required]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [Column("last_name")]
    [Required]
    [MaxLength(255)]
    public string LastName { get; set; }

    [Column("phone_number")]
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    [ForeignKey(nameof(Room))]
    [Column("room_id")]
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }

    [Column("check_in_date")]
    [Required]
    public DateTime CheckInDate { get; set; }

    [Column("check_out_date")]
    [Required]
    public DateTime CheckOutDate { get; set; }

    [Column("comment")]
    [MaxLength(500)]
    public string Comment { get; set; }
}