namespace Application.DTOs.BookDto;

public class GetBookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int PublishedYear { get; set; }
    public int CategoryId { get; set; }
}
