using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Lambda3.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Wiremock_Recording.Server.Api;
using WireMock.Server;
using WireMock.Settings;

namespace Wiremock_Recording.Test
{
    public class ForecastRecordingTests
    {
        private const string Folder = "../../../../Mappins";
        private WireMockServer _mockServer;
        private WebApplicationFactory<Startup> _factory;

        [OneTimeSetUp]
        public void Setup()
        {
            ClearPreviousStaticMappings();

            _factory = new WebApplicationFactory<Startup>(8000);
            _mockServer = WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] {"http://localhost:9095/"},
                ProxyAndRecordSettings = new ProxyAndRecordSettings
                {
                    SaveMapping = true,
                    Url = "http://localhost:8000/WeatherForecast",
                    SaveMappingForStatusCodePattern = "2xx",
                    AllowAutoRedirect = true,
                }
            });
        }

        private static void ClearPreviousStaticMappings()
        {
            if (!Directory.Exists(Folder)) return;
            
            var files = Directory.GetFiles(Folder);
            foreach (var filePath in files) File.Delete(filePath);
        }

        [OneTimeTearDown]
        public void StopServer()
        {
            _mockServer.SaveStaticMappings(Folder);
            _mockServer.Stop();
            
            _factory.Dispose();
        }

        [Test]
        public async Task Forecast()
        {
            using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost:9095")
            });
            
            var result = await client.GetAsync("WeatherForecast");
            result.Should().Be200Ok();
        }


        [Test]
        public async Task ForecastTomorrow()
        {
            using var client = new HttpClient();

            var result = await client.GetAsync("http://localhost:9095/WeatherForecast/tomorrow");

            result.Should().Be200Ok();
        }
    }
}