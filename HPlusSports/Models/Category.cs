﻿using System.ComponentModel.DataAnnotations;

namespace HPlusSports.Models
{
    public class Category
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }

        public long? ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}