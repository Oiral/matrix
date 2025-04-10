using AutoMapper;
using Matrix.Core.Exceptions;
using Matrix.Core.Models;
using Matrix.Entities;
using Matrix.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Matrix.Core.Services;

public class BikeService : BaseService
{
    private readonly MatrixDbContext _db;
    private readonly IMapper _mapper;

    public BikeService(MatrixDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new bike and associates it with the specified user email.
    /// </summary>
    /// <param name="email">The email of the user who owns the bike.</param>
    /// <param name="make">The make of the bike.</param>
    /// <param name="model">The model of the bike.</param>
    /// <param name="year">The year the bike was manufactured.</param>
    public async Task<Guid> CreateBike(string email, string? make, string? model, DateTime? year)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        var id = Guid.NewGuid();
        
        var newBike = new Bike
        {
            BikeId = id,
            Email = email,
            Make = make,
            Model = model,
            Year = year,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        await _db.Bikes.AddAsync(newBike);
        await _db.SaveChangesAsync();
        return id;
    }

    /// <summary>
    /// Updates the details of a bike in the database.
    /// </summary>
    /// <param name="bikeId">The unique identifier of the bike to update.</param>
    /// <param name="make">The new make of the bike, or null to leave unchanged.</param>
    /// <param name="model">The new model of the bike, or null to leave unchanged.</param>
    /// <param name="year">The new year of the bike, or null to leave unchanged.</param>
    public async Task UpdateBike(Guid bikeId, string? make, string? model, DateTime? year)
    {
        Bike? foundBike = await _db.Bikes.FirstOrDefaultAsync(b => b.BikeId == bikeId);
        if (foundBike == null)
        {
            throw new BikeNotFoundException($"The bike with the id {bikeId} cannot be found");
        }
        
        foundBike.Make = make ?? foundBike.Make;
        foundBike.Model = model ?? foundBike.Model;
        foundBike.Year = year ?? foundBike.Year;
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the given bike
    /// </summary>
    /// <param name="bikeId">The bike to delete</param>
    public async Task DeleteBike(Guid bikeId)
    {
        //We fetch the bike so that we can delete it and also fire any other logic that needs to happen on delete
        Bike? foundBike =  await _db.Bikes.FirstOrDefaultAsync(b => b.BikeId == bikeId);

        if (foundBike == null)
        {
            //We can just return instead of throwing an error as the id is not in our database
            return;
        }
        
        _db.Bikes.Remove(foundBike);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Get a list of the users bikes
    /// </summary>
    /// <param name="email">The email for the user</param>
    /// <returns>A list of all bikes that the user has created</returns>
    public async Task<List<BikeModel>> GetUsersBikes(string email)
    {
        List<Bike> usersBikes = await _db.Bikes.AsNoTracking().Where(b => b.Email == email).ToListAsync();

        return usersBikes.Count == 0
            ? new List<BikeModel>()
            : usersBikes.Select(ub => _mapper.Map<BikeModel>(ub)).ToList();
    }

    /// <summary>
    /// Get a list of all bikes.
    /// </summary>
    /// <returns>A list of Bikes representing all bikes, or an empty list if none exist.</returns>
    public async Task<List<BikeModel>> GetAllBikes()
    {
        List<Bike> usersBikes = await _db.Bikes.AsNoTracking().ToListAsync();

        return usersBikes.Count == 0
            ? new List<BikeModel>()
            : usersBikes.Select(ub => _mapper.Map<BikeModel>(ub)).ToList();
    }
}
