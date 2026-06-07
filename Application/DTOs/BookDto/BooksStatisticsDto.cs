namespace Application.DTOs.BookDto;

public class BookStatisticsDto
{
    public int TotalBooks { get; set; }
    public decimal AveragePrice { get; set; }
    public int TotalBorrows { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
}
