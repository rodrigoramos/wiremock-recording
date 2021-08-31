using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using WireMock.Server;
using WireMock.Settings;

namespace Wiremock_Playback.Test
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
            
            const string folder = "../../../../Mappins";
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

        [Test]
        public async Task DDA()
        {
            var client = new HttpClient();
            var result =
                await client.GetAsync(
                    "http://localhost:8080/abcbrasil.core.debitodiretoautorizado.api/api/v1/sacados/existe");

            result.Should().Be200Ok();
        }

        [Test]
        public async Task RegisterCity()
        {
            using var client = new HttpClient();
            var registerModel = new 
            {
                CityCode = 12,
                CityName = "Sao Paulo"
            };

            // var result = await client.PostAsJsonAsync(
            //     "http://localhost:9095/WeatherForecast/register-city",
            //     registerModel);
            
            // var content =  "{\"cityName\":\"São Paulo\",\"cityCode\":12}";
            const string content = "{\"cityName\":*,\"cityCode\":12}";
            var strContent = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await client.PostAsync(
                "http://localhost:9095/WeatherForecast/register-city",
                strContent);
            
            result.Should().Be200Ok();
        }
    }
}
