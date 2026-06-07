namespace Application.DTOs.CategoryDto;

public class GetCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

}
