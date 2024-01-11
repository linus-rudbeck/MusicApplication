using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace MusicApplication.Tests
{
    [TestFixture]
    public class AuthorizationIntegrationTests
    {
        WebApplicationFactory<Program> _factory;

        HttpClient _client;


        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureKestrel(options =>
                    {
                        options.ListenLocalhost(5001, listenOptions =>
                        {
                            listenOptions.UseHttps();
                        });
                    });
                });

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                BaseAddress = new Uri("https://localhost:5001"),
                AllowAutoRedirect = false
            });
        }


        [Test]
        public async Task Get_SecurePage_RedirectsToLogin()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/Songs/Create");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
            StringAssert.StartsWith("https://localhost:5001/Identity/Account/Login", response.Headers.Location.ToString());
        }

    } // https://localhost:5001/Identity/Account/Login
}
