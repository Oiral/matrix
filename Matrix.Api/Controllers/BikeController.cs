using AutoMapper;
using Matrix.Api.Requests;
using Matrix.Api.Responses;
using Matrix.Core.Models;
using Matrix.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Matrix.Api.Controllers;

/// <summary>
/// The controller for managing bikes.
/// </summary>
[Route("bike")]
public class BikeController : BaseController
{
    private readonly IMapper _mapper;
    private readonly BikeService _bikeService;
    private readonly EmailService _emailService;

    public BikeController(IMapper mapper, BikeService bikeService, EmailService emailService)
    {
        _mapper = mapper;
        _bikeService = bikeService;
        _emailService = emailService;
    }

    /// <summary>
    /// Gets a list of all bikes.
    /// </summary>
    /// <remarks>
    /// Fetch all bikes currently listed in the database.
    /// </remarks>
    /// <returns>A list of bikes.</returns>
    [HttpGet]
    public async Task<List<BikeResponse>> GetBikes()
    {
        //TODO Does this endpoint need more security e.g. only fetch bikes created by an email
        
        List<BikeModel> allBikes = await _bikeService.GetAllBikes();
        
        return allBikes.Select(b => _mapper.Map<BikeResponse>(b)).ToList();
    }

    /// <summary>
    /// Create a bike.
    /// </summary>
    /// <param name="request">The request to create the bike.</param>
    [HttpPost]
    public async Task<IActionResult> CreateBike([FromBody] CreateBikeRequest request)
    {
        if (!_emailService.IsValidEmail(request.Email))
        {
            return new BadRequestObjectResult("Ensure the email is in a correct format");
        }

        if (request.Make != null && string.IsNullOrWhiteSpace(request.Make))
        {
            return new BadRequestObjectResult("Ensure that Make is null or a valid make");
        }

        if (request.Model != null && string.IsNullOrWhiteSpace(request.Model))
        {
            return new BadRequestObjectResult("Ensure that Model is null or a valid make");
        }
        
        Guid bikeId = await _bikeService.CreateBike(request.Email, request.Make, request.Model, request.Year);

        return Created(bikeId.ToString(), bikeId.ToString());
    }

    /// <summary>
    /// Update a bikes details.
    /// </summary>
    /// <param name="bikeId">The bike id to update.</param>
    /// <param name="request">The request for the updated parameters.</param>
    [HttpPatch]
    [Route("{bikeId}")]
    public async Task<IActionResult> UpdateBike(Guid bikeId, [FromBody] UpdateBikeRequest request)
    {
        if (request.Make != null && string.IsNullOrWhiteSpace(request.Make))
        {
            return new BadRequestObjectResult("Ensure that Make is null or a valid make");
        }

        if (request.Model != null && string.IsNullOrWhiteSpace(request.Model))
        {
            return new BadRequestObjectResult("Ensure that Model is null or a valid make");
        }
        
        await _bikeService.UpdateBike(bikeId: bikeId, make: request.Make, model: request.Model, year: request.Year);

        return Ok();
    }

    /// <summary>
    /// Delete a bike.
    /// </summary>
    /// <param name="bikeId">The bike id to delete.</param>
    [HttpDelete]
    [Route("{bikeId}")]
    public async Task<IActionResult> DeleteBike(Guid bikeId)
    {
        await _bikeService.DeleteBike(bikeId);
        return Ok();
    }
}