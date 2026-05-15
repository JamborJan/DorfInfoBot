using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DorfInfoBot.API.Entities
{
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public string PreviewImage { get; set; }
        [ForeignKey("NewsId")]
        public News News { get; set; }
        public int NewsId { get; set; }
    }
}