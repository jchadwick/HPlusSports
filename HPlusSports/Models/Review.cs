using System.ComponentModel.DataAnnotations;

namespace HPlusSports.Models
{
    public class Review
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long ProductId { get; set; }

        [Range(minimum: 1, maximum: 5)]
        public int Rating { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}