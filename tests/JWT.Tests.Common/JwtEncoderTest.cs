using System.Collections.Generic;
using FluentAssertions;
using JWT.Algorithms;
using JWT.Serializers;
using JWT.Tests.Common;
using Xunit;

namespace JWT.Tests
{
    public class JwtEncoderTest
    {
        [Fact]
        public void Encode_Should_Encode_To_Token()
        {
            var serializer = new JsonNetSerializer();
            var urlEncoder = new JwtBase64UrlEncoder();
            var encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializer, urlEncoder);

            var actual = encoder.Encode(TestData.Customer, "ABC");

            actual.Should().Be(TestData.Token);
        }

        [Fact]
        public void Encode_Should_Encode_To_Token_With_Extra_Headers()
        {
            var serializer = new JsonNetSerializer();
            var urlEncoder = new JwtBase64UrlEncoder();
            var encoder = new JwtEncoder(new HMACSHA256Algorithm(), serializer, urlEncoder);

            var extraheaders = new Dictionary<string, object> { { "foo", "bar" } };
            var actual = encoder.Encode(extraheaders, TestData.Customer, "ABC");

            actual.Should().Be(TestData.ExtraHeadersToken);
        }
    }
}