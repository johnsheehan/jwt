﻿using System;
using FluentAssertions;
using JWT.Serializers;
using Xunit;
using JWT.Tests.Common;

namespace JWT.Tests
{
    public class DecodeTests
    {
        [Fact]
        public void Decode_Should_DecodeToken_To_Json_Encoded_String_With_JsonNet_Serializer()
        {
            var serializer = new JsonNetSerializer();
            JsonWebToken.JsonSerializer = serializer;

            var expectedPayload = serializer.Serialize(TestData.Customer);
            var actualPayload = JsonWebToken.Decode(TestData.Token, "ABC", verify: false);

            actualPayload.Should().Be(expectedPayload);
        }

        [Fact]
        public void DecodeToObject_Should_DecodeToken_To_Dictionary_With_JsonNet_Serializer()
        {
            JsonWebToken.JsonSerializer = new JsonNetSerializer();

            var actualPayload = JsonWebToken.DecodeToObject(TestData.Token, "ABC", verify: false);

            actualPayload.ShouldBeEquivalentTo(TestData.DictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [Fact]
        public void DecodeToObject_Should_DecodeToken_To_Generic_Type_With_JsonNet_Serializer()
        {
            JsonWebToken.JsonSerializer = new JsonNetSerializer();

            var actualPayload = JsonWebToken.DecodeToObject<Customer>(TestData.Token, "ABC", verify: false);

            actualPayload.ShouldBeEquivalentTo(TestData.Customer);
        }

        [Fact]
        public void DecodeToObject_Should_Throw_Exception_On_MalformedToken()
        {
            Action action = () => JsonWebToken.DecodeToObject<Customer>(TestData.MalformedToken, "ABC", verify: false);

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void DecodeToObject_Should_Throw_Exception_On_Invalid_Key()
        {
            Action action = () => JsonWebToken.DecodeToObject<Customer>(TestData.Token, "XYZ", verify: true);

            action.ShouldThrow<SignatureVerificationException>();
        }

        [Fact]
        public void DecodeToObject_Should_Throw_Exception_On_Invalid_Expiration_Claim()
        {
            var invalidexptoken = JsonWebToken.Encode(new { exp = "asdsad" }, "ABC", JwtHashAlgorithm.HS256);

            Action action = () => JsonWebToken.DecodeToObject<Customer>(invalidexptoken, "ABC", verify: true);

            action.ShouldThrow<SignatureVerificationException>();
        }

        [Fact]
        public void DecodeToObject_Should_Throw_Exception_On_Expired_Claim()
        {
            var hourAgo = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0));
            var unixTimestamp = (int)(hourAgo - new DateTime(1970, 1, 1)).TotalSeconds;
            var expiredtoken = JsonWebToken.Encode(new { exp = unixTimestamp }, "ABC", JwtHashAlgorithm.HS256);

            Action action = () => JsonWebToken.DecodeToObject<Customer>(expiredtoken, "ABC", verify: true);

            action.ShouldThrow<TokenExpiredException>();
        }

        [Fact]
        public void DecodeToObject_Should_Throw_Exception_Before_NotBefore_Becomes_Valid()
        {
            var nbf = (int)(DateTime.UtcNow.AddHours(1) - JwtValidator.UnixEpoch).TotalSeconds;
            var invalidnbftoken = JsonWebToken.Encode(new { nbf = nbf }, "ABC", JwtHashAlgorithm.HS256);

            Action action = () => JsonWebToken.DecodeToObject<Customer>(invalidnbftoken, "ABC", verify: true);

            action.ShouldThrow<SignatureVerificationException>();
        }

        [Fact]
        public void DecodeToObject_Should_Decode_Token_After_NotBefore_Becomes_Valid()
        {
            var nbf = (int)(DateTime.UtcNow - JwtValidator.UnixEpoch).TotalSeconds;
            var validnbftoken = JsonWebToken.Encode(new { nbf = nbf }, "ABC", JwtHashAlgorithm.HS256);

            JsonWebToken.DecodeToObject<Customer>(validnbftoken, "ABC", verify: true);
        }
    }
}