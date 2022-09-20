using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Contracts.Database;

[Table("tbl_rooms", Schema = "public")]
public class Room
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("type")]
    public string Type { get; set; }

    [Required]
    [Column("price")]
    public int Price { get; set; }
}