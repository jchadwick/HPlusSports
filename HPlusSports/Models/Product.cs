using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HPlusSports.Models
{
  public class Product
  {
    public long Id { get; set; }

    [Range(1, long.MaxValue)]
    public long CategoryId { get; set; }

    [Required]
    public string SKU { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string Summary { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [DataType(DataType.Currency)]
    [Range(minimum: 0, maximum: double.MaxValue)]
    public double MSRP { get; set; }

    [DataType(DataType.Currency)]
    [Range(minimum: 0, maximum: double.MaxValue)]
    public double Price { get; set; }

    [Required]
    [Display(Name = "Last Updated By")]
    public DateTime LastUpdated { get; set; } 

    [Required]
    [Display(Name = "Last Update Timestamp")]
    public string LastUpdatedUserId { get; set; }

    [NotMapped]
    public string[] Tags
    {
      get { return _tags.Split(';'); }
      set { _tags = string.Join(";", value ?? Enumerable.Empty<string>()); }
    }

    [Column("Tags")]
    internal string _tags = string.Empty;

    public virtual Category Category { get; set; }

    public virtual ICollection<Image> Images { get; private set; }

    public Product()
    {
      Images = new List<Image>();
    }
  }
}