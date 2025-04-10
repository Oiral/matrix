using FluentAssertions;
using Matrix.Core.Services;
using Xunit;

namespace Matrix.Test.Services;

public class EmailServiceTests
{
    private readonly EmailService _emailService;

    public EmailServiceTests(EmailService emailService)
    {
        _emailService = emailService;
    }

    [InlineData("Hamish@pageland.co.nz")]
    [InlineData("test@domain.com")]
    [InlineData("test@short.ab")]
    [InlineData("test123@domain.com")]
    [InlineData("test123@domain.school.co.nz")]
    [Theory]
    public void IsValidEmail_IsValid(string email)
    {
        _emailService.IsValidEmail(email).Should().BeTrue();
    }

    [InlineData("NotAnEmail")]
    [InlineData("test123@domain")]
    [InlineData("test@test@domain.com")]
    [Theory]
    public void IsValidEmail_NoValid(string email)
    {
        _emailService.IsValidEmail(email).Should().BeFalse();
    }
}