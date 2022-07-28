using System.ComponentModel.DataAnnotations;
using Blog.Contracts;

namespace Blog.Database.Models;

#nullable disable
public class Blog
{
    [Key]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(Constants.Lengths.IdField)]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(Constants.Lengths.TextField)]
    public string Organization { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(Constants.Lengths.TextField)]
    public string Name { get; set; }
}

#nullable restore