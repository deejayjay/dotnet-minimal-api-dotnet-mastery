using System.ComponentModel.DataAnnotations;

namespace TangyAPI.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public required string Name { get; set; }

    public DateTime AddedDate { get; set; }
}