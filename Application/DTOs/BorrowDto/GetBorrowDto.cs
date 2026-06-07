namespace Application.DTOs.BorrowDto;

public class GetBorrowDto
{
    public int Id { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
}
