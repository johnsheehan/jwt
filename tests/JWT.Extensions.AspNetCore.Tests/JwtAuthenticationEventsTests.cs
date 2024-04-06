namespace JWT.Extensions.AspNetCore.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using JWT.Algorithms;
    using JWT.Tests.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JwtAuthenticationEventsTests
    {
        private static readonly Fixture _fixture = new Fixture();
        private static TestServer _server;

        private HttpClient _client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var options = new JwtAuthenticationOptions
            {
                Keys = TestData.Secrets,
                VerifySignature = true
            };
            _server = CreateServer(options);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _server.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _client = _server.CreateClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client.Dispose();
        }

        [TestMethod]
        public async Task Request_Should_Return_Ok_When_Token_Is_Valid()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                JwtAuthenticationDefaults.AuthenticationScheme,
                TestData.TokenByAsymmetricAlgorithm);

            // Act
            var response = await _client.GetAsync("https://example.com/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            MyEventsDependency.HandledSuccessfulTicket.Should().BeTrue();
        }

        private static TestServer CreateServer(JwtAuthenticationOptions configureOptions)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseAuthentication();

                    app.Use(async (HttpContext context, Func<Task> next) =>
                    {
                        var authenticationResult = await context.AuthenticateAsync();
                        if (authenticationResult.Succeeded)
                        {
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            context.Response.ContentType = new ContentType("text/json").MediaType;

                            await context.Response.WriteAsync("Hello");
                        }
                        else
                        {
                            await context.ChallengeAsync();
                        }
                    });
                })
                .ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                        {
                            // Prevents from System.InvalidOperationException: No authenticationScheme was specified, and there was no DefaultAuthenticateScheme found.
                            options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;

                            // Prevents from System.InvalidOperationException: No authenticationScheme was specified, and there was no DefaultChallengeScheme found.
                            options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                        })
                        .AddJwt(options =>
                        {
                            options.Keys = null;
                            options.VerifySignature = configureOptions.VerifySignature;
                            options.EventsType = typeof(MyEvents);
                        });
                    services.AddTransient<MyEventsDependency>();
                    services.AddTransient<MyEvents>();
                    services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(TestData.RS256Algorithm));
                });

            return new TestServer(builder);
        }

        public class MyEventsDependency
        {
            public static bool HandledSuccessfulTicket = false;

            public void Mark()
            {
                HandledSuccessfulTicket = true;
            }
        }

        public class MyEvents : JwtAuthenticationEvents
        {
            private readonly MyEventsDependency _dependency;

            public MyEvents(MyEventsDependency dependency)
            {
                _dependency = dependency;
            }

            public override AuthenticateResult SuccessfulTicket(ILogger logger, AuthenticationTicket ticket)
            {
                _dependency.Mark();
                return base.SuccessfulTicket(logger, ticket);
            }
        }
    }
}
