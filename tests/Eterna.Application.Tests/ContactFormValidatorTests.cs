using Eterna.Application.Contacts;

namespace Eterna.Application.Tests;

public sealed class ContactFormValidatorTests
{
    private readonly ContactFormValidator _validator = new();

    [Fact]
    public async Task Missing_name_fails()
    {
        var request = Valid();
        request.Name = "";
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Name));
    }

    [Fact]
    public async Task Missing_email_fails()
    {
        var request = Valid();
        request.Email = "";
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Email));
    }

    [Fact]
    public async Task Invalid_email_fails()
    {
        var request = Valid();
        request.Email = "not-an-email";
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Email));
    }

    [Fact]
    public async Task Missing_service_fails()
    {
        var request = Valid();
        request.Service = "";
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Service));
    }

    [Fact]
    public async Task Unknown_service_fails()
    {
        var request = Valid();
        request.Service = "Branding";
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Service));
    }

    [Fact]
    public async Task Missing_message_fails()
    {
        var request = Valid();
        request.Message = "";
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Message));
    }

    [Fact]
    public async Task Name_over_120_fails()
    {
        var request = Valid();
        request.Name = new string('A', 121);
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Name));
    }

    [Fact]
    public async Task Company_over_160_fails()
    {
        var request = Valid();
        request.Company = new string('A', 161);
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Company));
    }

    [Fact]
    public async Task Email_over_254_fails()
    {
        var request = Valid();
        request.Email = new string('a', 243) + "@eterna.test";
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Email));
    }

    [Fact]
    public async Task Phone_over_40_fails()
    {
        var request = Valid();
        request.Phone = new string('1', 41);
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Phone));
    }

    [Fact]
    public async Task Budget_over_40_fails()
    {
        var request = Valid();
        request.Budget = new string('A', 41);
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Budget));
    }

    [Fact]
    public async Task Message_over_4000_fails()
    {
        var request = Valid();
        request.Message = new string('A', 4001);
        var result = await _validator.ValidateAsync(request);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ContactFormRequest.Message));
    }

    [Fact]
    public async Task Valid_request_passes()
    {
        var result = await _validator.ValidateAsync(Valid());
        Assert.True(result.IsValid);
    }

    internal static ContactFormRequest Valid() => new()
    {
        Name = "Amira Hassan",
        Company = "Example Co",
        Email = "amira@example.com",
        Phone = "+20 1000000000",
        Service = "Website",
        Budget = "To be scoped",
        Message = "We need a durable storefront."
    };
}
