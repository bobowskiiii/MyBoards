using Microsoft.EntityFrameworkCore;

namespace MyBoardss.Entities;

public class Address
{
    public Guid Id { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string PostalCode { get; set; }
    public required User User { get; set; }
    public Guid UserId { get; set; }
    public Coordinate Coordinate { get; set; }
}


[Owned]
public class Coordinate
{
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}