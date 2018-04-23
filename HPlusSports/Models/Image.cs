using System;
using System.ComponentModel.DataAnnotations;

namespace HPlusSports.Models
{
  public class Image
  {
    public long Id { get; set; }

    [Required]
    public byte[] Content { get; set; }

    [Required]
    public string ContentType { get; set; }
  }
}