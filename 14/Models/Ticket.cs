namespace Cinema.Models;

public class Ticket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int SeatId { get; set; }
}
