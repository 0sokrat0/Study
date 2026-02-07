using System;

namespace Cinema.Models;

public class Session
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int HallId { get; set; }
    public DateTime DateTime { get; set; }
}
