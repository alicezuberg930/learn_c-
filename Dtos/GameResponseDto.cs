namespace test.dtos;

public record class GameResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Genre { get; set; }
    public DateOnly ReleaseDate { get; set; }
}
