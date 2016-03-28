﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using FluentAssertions;

namespace JWT.Tests
{
    [TestClass]
    public class DecodeTests
    {
        private static readonly Customer customer = new Customer { FirstName = "Bob", Age = 37 };

        private const string token =          "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        private const string malformedtoken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";

        private static readonly IDictionary<string, object> dictionaryPayload = new Dictionary<string, object>
        { 
            { "FirstName", "Bob" },
            { "Age", 37 }
        };

        [TestMethod]
        public void Should_Decode_Token_To_Json_Encoded_String()
        {
            var jsonSerializer = new JavaScriptSerializer();
            var expectedPayload = jsonSerializer.Serialize(customer);

            string decodedPayload = JsonWebToken.Decode(token, "ABC", false);

            Assert.AreEqual(expectedPayload, decodedPayload);
        }

        [TestMethod]
        public void Should_Decode_Token_To_Dictionary()
        {
            object decodedPayload = JsonWebToken.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [TestMethod]
        public void Should_Decode_Token_To_Dictionary_With_ServiceStack()
        {
            JsonWebToken.JsonSerializer = new ServiceStackJsonSerializer();

            object decodedPayload = JsonWebToken.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [TestMethod]
        public void Should_Decode_Token_To_Dictionary_With_Newtonsoft()
        {
            JsonWebToken.JsonSerializer = new NewtonJsonSerializer();

            object decodedPayload = JsonWebToken.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [TestMethod]
        public void Should_Decode_Token_To_Generic_Type()
        {
            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [TestMethod]
        public void Should_Decode_Token_To_Generic_Type_With_ServiceStack()
        {
            JsonWebToken.JsonSerializer = new ServiceStackJsonSerializer();

            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [TestMethod]
        public void Should_Decode_Token_To_Generic_Type_With_Newtonsoft()
        {
            JsonWebToken.JsonSerializer = new NewtonJsonSerializer();

            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Throw_On_Malformed_Token()
        {
            JsonWebToken.DecodeToObject<Customer>(malformedtoken, "ABC", false);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Tampered_Token_PayloadPart()
        {
            var parts = token.Split('.');
            
            var tamperedToken = parts[0] + "." + "abc" + parts[1].Substring(3) + "." + parts[2];

            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(tamperedToken, "ABC", true);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Invalid_Key()
        {
            string invalidkey = "XYZ";

            JsonWebToken.DecodeToObject<Customer>(token, invalidkey, true);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Invalid_Expiration_Claim()
        {
            var invalidexptoken = JsonWebToken.Encode(new { exp = "asdsad" }, "ABC", JwtHashAlgorithm.HS256);

            JsonWebToken.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Expired_Token()
        {
            var anHourAgoUtc = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0));
            Int32 unixTimestamp = (Int32)(anHourAgoUtc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var invalidexptoken = JsonWebToken.Encode(new { exp = unixTimestamp }, "ABC", JwtHashAlgorithm.HS256);

            JsonWebToken.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Before_NotBefore_Token()
        {
            var anHourFromNowUtc = DateTime.UtcNow.Add(new TimeSpan(1, 0, 0));
            Int32 unixTimestamp = (Int32)(anHourFromNowUtc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var invalidnbftoken = JsonWebToken.Encode(new { nbf = unixTimestamp }, "ABC", JwtHashAlgorithm.HS256);

            JsonWebToken.DecodeToObject<Customer>(invalidnbftoken, "ABC", true);
        }

        [TestMethod]
        public void Should_Decode_Token_After_NotBefore_Token_Becomes_Valid()
        {
            var nowUtc = DateTime.UtcNow;
            Int32 unixTimestamp = (Int32)(nowUtc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var validnbftoken = JsonWebToken.Encode(new { nbf = unixTimestamp }, "ABC", JwtHashAlgorithm.HS256);

            JsonWebToken.DecodeToObject<Customer>(validnbftoken, "ABC", true);
        }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public int Age { get; set; }
    }
}