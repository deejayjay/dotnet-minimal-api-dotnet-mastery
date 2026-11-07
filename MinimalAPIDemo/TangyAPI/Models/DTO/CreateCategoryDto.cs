using System.ComponentModel.DataAnnotations;

namespace TangyAPI.Models.DTO;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public required string Name { get; set; }
}
