using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using WireMock.Server;
using WireMock.Settings;

namespace Wiremock_Recording.Test
{
    public class ForecastPlaybackTests
    {
        private WireMockServer _mockServer;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockServer = WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] {"http://localhost:9095/"},
            });
            
            const string folder = "../../../Mappins";
            _mockServer.ReadStaticMappings(folder);
        }

        [OneTimeTearDown]
        public void StopServer()
        {
            _mockServer.Stop();
        }

        [Test]
        public async Task Forecast()
        {
            var client = new HttpClient();

            var result = await client.GetAsync("http://localhost:9095/WeatherForecast");

            result.Should().Be200Ok();
        }

        [Test]
        public async Task ForecastTomorrow()
        {
            var client = new HttpClient();

            var result = await client.GetAsync("http://localhost:9095/WeatherForecast/tomorrow");

            result.Should().Be200Ok();
        }
    }
}
