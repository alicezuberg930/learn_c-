using System.ComponentModel.DataAnnotations;
namespace test.dtos;

public record class CreateGameRequestDto
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Genre { get; set; } = string.Empty;
    [Required] public decimal Price { get; set; }
    [Required] public DateOnly ReleaseDate { get; set; }
}
