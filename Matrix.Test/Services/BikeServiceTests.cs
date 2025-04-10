using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Matrix.Core.Models;
using Matrix.Core.Services;
using Matrix.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Matrix.Test.Services;

public class BikeServiceTests 
{
    private readonly BikeService _bikeService;

    public BikeServiceTests(BikeService bikeService)
    {
        _bikeService = bikeService;
    }

    [Fact]
    public async Task CreateBike_ShouldAddBikeToDatabase()
    {
        // Arrange
        var email = "test@example.com";
        var make = "TestMake";
        var model = "TestModel";
        DateTime year = DateTime.Now;

        // Act
        Guid bikeId = await _bikeService.CreateBike(email, make, model, year);

        // Assert
        Assert.NotEqual(Guid.Empty, bikeId);
    }

    [Fact]
    public async Task UpdateBike_ShouldUpdateBikeDetails()
    {
        // Arrange
        var email = "test1@example.com";
        Guid bikeId = await _bikeService.CreateBike(email, "OldMake", "OldModel", DateTime.Now.AddYears(-1));

        // Act
        await _bikeService.UpdateBike(bikeId, "NewMake", "NewModel", DateTime.Now);

        // Assert
        List<BikeModel> updatedBikes = await _bikeService.GetUsersBikes(email);
        updatedBikes.Should().NotContain(b => b.Make == "OldMake" && b.Model == "OldModel");
        updatedBikes.Where(b => b.BikeId == bikeId).Should().HaveCount(1);
        updatedBikes.Where(b => b.BikeId == bikeId).Should().Contain(b => b.Make == "NewMake" && b.Model == "NewModel");
    }

    [Fact]
    public async Task DeleteBike_ShouldRemoveBikeFromDatabase()
    {
        // Arrange
        var email = "test2@example.com";
        Guid bikeId = await _bikeService.CreateBike(email, "Make", "Model", DateTime.Now);

        // Act
        await _bikeService.DeleteBike(bikeId);

        // Assert
        List<BikeModel> bikes = await _bikeService.GetUsersBikes(email);
        bikes.Should().NotContain(b => b.BikeId == bikeId);
    }

    [Fact]
    public async Task GetUsersBikes_ShouldReturnListOfBikes()
    {
        // Arrange
        var email = "test3@example.com";
        Guid id1 = await _bikeService.CreateBike(email, "Make1", "Model1", DateTime.Now);
        Guid id2 = await _bikeService.CreateBike(email, "Make2", "Model2", DateTime.Now);

        // Act
        List<BikeModel> result = await _bikeService.GetUsersBikes(email);

        // Assert
        result.Should().Contain(b => b.BikeId == id1 && b.Make == "Make1" && b.Model == "Model1");
        result.Should().Contain(b => b.BikeId == id2 && b.Make == "Make2" && b.Model == "Model2");
        result.Count(b => b.Email == email).Should().Be(2);
    }

    [Fact]
    public async Task GetAllBikes_ShouldReturnListOfAllBikes()
    {
        // Arrange
        var email1 = "test4@example.com";
        var email2 = "test5@example.com";
        await _bikeService.CreateBike(email1, "Make1", "Model1", DateTime.Now);
        await _bikeService.CreateBike(email2, "Make2", "Model2", DateTime.Now);

        // Act
        List<BikeModel> result = await _bikeService.GetAllBikes();

        // Assert
        result.Should().Contain(b => b.Email == email1 && b.Make == "Make1" && b.Model == "Model1");
        result.Should().Contain(b => b.Email == email2 && b.Make == "Make2" && b.Model == "Model2");
    }

    [Fact]
    public async Task CreateBike_WithNullMake_ShouldAddBikeToDatabase()
    {
        // Arrange
        var email = "test@example.com";
        string? make = null;
        var model = "TestModel";
        DateTime year = DateTime.Now;

        // Act
        Guid bikeId = await _bikeService.CreateBike(email, make, model, year);

        // Assert
        Assert.NotEqual(Guid.Empty, bikeId);
    }

    [Fact]
    public async Task CreateBike_WithNullModel_ShouldAddBikeToDatabase()
    {
        // Arrange
        var email = "test@example.com";
        var make = "TestMake";
        string? model = null;
        DateTime year = DateTime.Now;

        // Act
        Guid bikeId = await _bikeService.CreateBike(email, make, model, year);

        // Assert
        Assert.NotEqual(Guid.Empty, bikeId);
    }

    [Fact]
    public async Task UpdateBike_WithNullMake_ShouldNotChangeMake()
    {
        // Arrange
        var email = "test@example.com";
        Guid bikeId = await _bikeService.CreateBike(email, "OldMake", "OldModel", DateTime.Now);

        // Act
        await _bikeService.UpdateBike(bikeId, null, "NewModel", DateTime.Now);

        // Assert
        List<BikeModel> updatedBikes = await _bikeService.GetUsersBikes(email);
        updatedBikes.Where(b => b.BikeId == bikeId).Should().Contain(b => b.Make == "OldMake" && b.Model == "NewModel");
    }

    [Fact]
    public async Task UpdateBike_WithNullModel_ShouldNotChangeModel()
    {
        // Arrange
        var email = "test@example.com";
        Guid bikeId = await _bikeService.CreateBike(email, "OldMake", "OldModel", DateTime.Now);

        // Act
        await _bikeService.UpdateBike(bikeId, "NewMake", null, DateTime.Now);

        // Assert
        List<BikeModel> updatedBikes = await _bikeService.GetUsersBikes(email);
        updatedBikes.Where(b => b.BikeId == bikeId).Should().Contain(b => b.Make == "NewMake" && b.Model == "OldModel");
    }
}
