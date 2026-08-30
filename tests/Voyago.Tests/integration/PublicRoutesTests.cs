using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Voyago.Tests.Integration;

public sealed class PublicRoutesTests
    : IClassFixture<VoyagoWebApplicationFactory>
{
    private readonly VoyagoWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public PublicRoutesTests(
        VoyagoWebApplicationFactory factory)
    {
        _factory = factory;

        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });
    }

    //public PublicRoutesTests(
    //    VoyagoWebApplicationFactory factory)
    //{
    //    _client = factory.CreateClient(
    //        new WebApplicationFactoryClientOptions
    //        {
    //            AllowAutoRedirect = false,
    //            BaseAddress = new Uri("https://localhost")
    //        });
    //}

    [Theory]
    [InlineData("/")]
    [InlineData("/Destinations")]
    [InlineData("/Destinations/1")]
    [InlineData("/Destinations/6")]
    [InlineData("/Privacy")]
    [InlineData("/api/v1/health")]
    public async Task PublicRoute_ReturnsSuccess(
        string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("/Destinations/999")]
    [InlineData("/Destinations/no-es-id")]
    public async Task InvalidDestination_ReturnsNotFound(
        string url)
    {
        var response = await _client.GetAsync(url);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Theory]
    [InlineData("Patagonia")]
    [InlineData("patagonia")]
    [InlineData("PATAGONIA")]
    public async Task DestinationSearch_IsCaseInsensitive(
        string search)
    {
        var encodedSearch =
            Uri.EscapeDataString(search);

        var response = await _client.GetAsync(
            $"/Destinations?Search={encodedSearch}");

        var html =
            await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        Assert.Contains("Patagonia", html);

        Assert.DoesNotContain(
            "No encontramos destinos",
            html);
    }

    [Theory]
    [InlineData("/images/destinations/1.webp")]
    [InlineData("/images/destinations/6.webp")]
    [InlineData("/images/packages/1.webp")]
    [InlineData("/images/hotels/1.webp")]
    [InlineData("/images/placeholders/destination.svg")]
    public async Task StaticAsset_Exists(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AnonymousAdmin_RedirectsToLoginWithReturnUrl()
    {
        var response = await _client.GetAsync("/Admin");

        Assert.Equal(
            HttpStatusCode.Redirect,
            response.StatusCode);

        var location =
            response.Headers.Location?.ToString();

        Assert.NotNull(location);

        Assert.Contains(
            "/Identity/Account/Login",
            location);

        Assert.Contains(
            "ReturnUrl=%2FAdmin",
            location);
    }

    [Fact]
    public async Task DestinationDetail_LoginLinkPreservesReturnUrl()
    {
        var response = await _client.GetAsync(
            "/Destinations/6");

        response.EnsureSuccessStatusCode();

        var html = await response.Content
            .ReadAsStringAsync();

        var decodedHtml =
            System.Net.WebUtility.HtmlDecode(html);

        var normalizedHtml =
            Uri.UnescapeDataString(decodedHtml);

        Assert.Contains(
            "/Identity/Account/Login",
            normalizedHtml,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "returnUrl=/Destinations/6",
            normalizedHtml,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AddFavorite_WithoutAntiforgery_IsRejected()
    {
        var response = await _client.PostAsync(
            "/Destinations/6?handler=AddFavorite",
            new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("id", "6"),
                new KeyValuePair<string, string>(
                    "returnUrl",
                    "/Destinations/6")
            ]));

        Assert.True(
            response.StatusCode is
                HttpStatusCode.BadRequest or
                HttpStatusCode.Redirect);
    }

    [Fact]
    public void Factory_UsesUniqueTemporaryDatabase()
    {
        Assert.Contains(
            "voyago-tests-",
            Path.GetFileName(_factory.DatabasePath));

        Assert.EndsWith(
            ".db",
            _factory.DatabasePath);

        Assert.NotEqual(
            Path.GetFullPath("Voyago/Data/voyago.db"),
            Path.GetFullPath(_factory.DatabasePath));
    }
}