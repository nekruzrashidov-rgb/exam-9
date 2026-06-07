namespace Application.DTOs.CategoryDto;

public class GetCategoryFillterDto
{
    public string Name { get; set; } = string.Empty;
    public int Page { get; set; }
    public int PageSize { get; set; }
}
