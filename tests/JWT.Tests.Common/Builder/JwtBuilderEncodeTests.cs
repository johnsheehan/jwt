using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoFixture;
using FluentAssertions;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JWT.Tests.Builder
{
    [TestClass]
    public class JwtBuilderEncodeTests
    {
        private static readonly Fixture _fixture = new Fixture();

        [TestMethod]
        public void Encode_With_Secret_Should_Return_Valid_Token()
        {
            var algorithm = new HMACSHA256Algorithm();
            var secret = _fixture.Create<string>();

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(algorithm)
                                  .WithSecret(secret)
                                  .Encode();

            token.Should()
                 .NotBeNullOrEmpty("because the token should contains some data");
            token.Split('.')
                 .Should()
                 .HaveCount(3, "because the token should consist of three parts");
        }

        [TestMethod]
        public void Encode_With_Secret_And_Payload_Should_Return_Valid_Token()
        {
            var algorithm = new HMACSHA256Algorithm();

            const ClaimName claimKey = ClaimName.ExpirationTime;
            var claimValue = DateTime.UtcNow.AddHours(1).ToString(CultureInfo.InvariantCulture);
            var secret = _fixture.Create<string>();

            var token = JwtBuilder.Create()
                                   .WithAlgorithm(algorithm)
                                   .WithSecret(secret)
                                   .AddClaim(claimKey, claimValue)
                                   .Encode();

            token.Should()
                 .NotBeNullOrEmpty("because the token should contains some data");

            token.Split('.')
                 .Should()
                 .HaveCount(3, "because the token should consist of three parts");
        }

        [TestMethod]
        public void Encode_With_PayloadWithClaims_Should_Return_Token()
        {
            var algorithm = new HMACSHA256Algorithm();
            var secret = _fixture.Create<string>();
            var claims = _fixture.Create<Dictionary<string, string>>();

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(algorithm)
                                  .WithSecret(secret)
                                  .AddClaims(claims)
                                  .Encode();

            var decodedToken = new UTF8Encoding(false)
               .GetString(new JwtBase64UrlEncoder()
                             .Decode(token.Split('.')[1]));

            token.Should()
                 .NotBeNullOrEmpty("because the token should contains some data");

            token.Split('.')
                 .Should()
                 .HaveCount(3, "because the token should consist of three parts");

            decodedToken.Should()
                        .ContainAll(claims.Keys, "because all used keys should be retrieved in the token");

            decodedToken.Should()
                        .ContainAll(claims.Values, "because all values associated with the claims should be retrieved in the token");
        }

        [TestMethod]
        public void Encode_Without_Dependencies_Should_Throw_Exception()
        {
            Action action = () =>
                JwtBuilder.Create()
                          .Encode();

            action.Should()
                  .Throw<InvalidOperationException>("because a JWT can't be built without dependencies");
        }

        [TestMethod]
        public void Encode_With_SymmetricAlgorithm_WithoutSecret_Should_Throw_Exception()
        {
            var algorithm = new HMACSHA256Algorithm();

            Action action =
                () => JwtBuilder.Create()
                                .WithAlgorithm(algorithm)
                                .Encode();

            action.Should()
                  .Throw<ArgumentNullException>("because a JWT can't be built with a symmetric algorithm and without a secret");
        }

        [TestMethod]
        public void Encode_WithoutAlgorithm_WithSecret_Should_Throw_Exception()
        {
            var secret = _fixture.Create<string>();

            Action action = () =>
                JwtBuilder.Create()
                          .WithSecret(secret)
                          .Encode();

            action.Should()
                  .Throw<InvalidOperationException>("because a JWT should not be created if no algorithm is provided");
        }

        [TestMethod]
        public void Encode_With_MultipleSecrets_Should_Throw_Exception()
        {
            var secrets = _fixture.Create<string[]>();

            Action action = () =>
                JwtBuilder.Create()
                          .WithSecret(secrets)
                          .Encode();

            action.Should()
                  .Throw<InvalidOperationException>("because a JWT should not be created if no algorithm is provided");
        }

        [TestMethod]
        public void Encode_WithoutAlgorithm_Should_Return_Token()
        {
            var secret = _fixture.Create<string>();

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new NoneAlgorithm())
                                  .WithSecret(secret)
                                  .Encode();

            token.Should()
                 .NotBeNullOrEmpty("because the token should contains some data");
            token.Split('.')
                 .Should()
                 .HaveCount(3, "because the token should consist of three parts");
            token.Split('.')
                 .Last()
                 .Should()
                 .BeEmpty("Because it should miss signature");
        }

        [TestMethod]
        public void Encode_Should_Return_Token_With_Extra_Headers()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new HMACSHA256Algorithm())
                                  .WithSecret(key)
                                  .AddHeader(HeaderName.KeyId, "42")
                                  .AddClaim(nameof(TestData.Customer.FirstName), TestData.Customer.FirstName)
                                  .AddClaim(nameof(TestData.Customer.Age), TestData.Customer.Age)
                                  .Encode();

            token.Should()
                 .Be(TestData.TokenWithCustomTypeHeader2, "because the same data encoded with the same key must result in the same token");
        }

        [TestMethod]
        public void Encode_Should_Return_Token_With_Custom_Extra_Headers()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new HMACSHA256Algorithm())
                                  .WithSecret(key)
                                  .AddHeader("version", 1)
                                  .AddClaim(nameof(TestData.Customer.FirstName), TestData.Customer.FirstName)
                                  .AddClaim(nameof(TestData.Customer.Age), TestData.Customer.Age)
                                  .Encode();

            token.Should()
                 .Be(TestData.TokenWithCustomTypeHeader3, "because the same data encoded with the same key must result in the same token");
        }
        
        [TestMethod]
        public void Encode_Should_Return_Token_With_Custom_Extra_Headers_Full_Payload()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new HMACSHA256Algorithm())
                                  .WithSecret(key)
                                  .AddHeader("version", 1)
                                  .Encode(TestData.Customer);

            token.Should()
                 .Be(TestData.TokenWithCustomTypeHeader3, "because the same data encoded with the same key must result in the same token");
        }
        
        [TestMethod]
        public void Encode_Should_Return_Token_With_Custom_Extra_Headers_Full_Payload_And_Claims()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new HMACSHA256Algorithm())
                                  .WithSecret(key)
                                  .AddHeader("version", 1)
                                  .AddClaim("ExtraClaim", "ValueClaim")
                                  .Encode(TestData.Customer);

            token.Should()
                 .Be(TestData.TokenWithCustomTypeHeader3AndClaim, "because the same data encoded with the same key must result in the same token");
        }
        
        [TestMethod]
        public void Encode_Should_Return_Token_With_Custom_Extra_Headers_Full_Payload_And_Claims_With_Nested()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new HMACSHA256Algorithm())
                                  .WithSecret(key)
                                  .AddHeader("version", 1)
                                  .AddClaim("ExtraClaim", new { NestedProperty1 = "Foo", NestedProperty2 = 3 })
                                  .Encode(TestData.Customer);

            token.Should()
                 .Be(TestData.TokenWithCustomTypeHeader3AndClaimNested, "because the same data encoded with the same key must result in the same token");
        }
        
        [TestMethod]
        public void Encode_Should_Return_Token_With_Custom_Extra_Headers_Full_Payload2()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new HMACSHA256Algorithm())
                                  .WithSecret(key)
                                  .AddHeader("version", 1)
                                  .Encode(
                                      new
                                      {
                                          ExtraClaim = new
                                          {
                                              NestedProperty1 = "Foo",
                                              NestedProperty2 = 3
                                          },
                                          FirstName = "Jesus",
                                          Age = 33
                                      });

            token.Should()
                .Be(TestData.TokenWithCustomTypeHeader3AndClaimNested, "because the same data encoded with the same key must result in the same token");
        }
        
        [TestMethod]
        public void Encode_Should_Return_Token_Nested_Data()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithAlgorithm(new HMACSHA256Algorithm())
                                  .WithSecret(key)
                                  .AddClaim<Customer>("Data", TestData.Customer)
                                  .Encode();

            token.Should()
                 .Be(TestData.TokenWithNestedData, "because the same data encoded with the same key must result in the same token");
        }

        [TestMethod]
        public void Encode_With_Custom_Factory_Return_Token()
        {
            const string key = TestData.Secret;

            var token = JwtBuilder.Create()
                                  .WithSecret(key)
                                  .WithAlgorithmFactory(new CustomFactory())
                                  .Encode();

            token.Should()
                 .NotBeNullOrEmpty("because the token should contains some data");
        }

        private sealed class CustomFactory : IAlgorithmFactory
        {
            public IJwtAlgorithm Create(JwtDecoderContext context) =>
                new HMACSHA256Algorithm();
        }
    }
}
