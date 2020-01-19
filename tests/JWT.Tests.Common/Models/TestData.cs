﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using JWT.Algorithms;

namespace JWT.Tests.Common.Models
{
    public static class TestData
    {
        public static readonly Customer Customer = new Customer
        {
            FirstName = "Jesus",
            Age = 33
        };

        public const string Secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public const string Secret2 = "QWORIJkmQWEDIHbjhOIHAUSDFOYnUGWEYT";

        public static string[] Secrets = { Secret, Secret2 };

        public const string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJKZXN1cyIsIkFnZSI6MzN9.jBdQNPhChZpZSMZX6Z5okc7YJ3dc5esWp4YCtasYXFU";
        public const string TokenWithExtraHeaders = "eyJmb28iOiJiYXIiLCJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJKZXN1cyIsIkFnZSI6MzN9.QQJaPxDE6E7l-zC-LKTbEgPfId5FDvowRKww1o6jdwU";
        public const string TokenWithIncorrectSignature = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        public const string TokenWithoutHeader = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.oj1ROhq6SyGDG3C0WIPe8wDuMJjA47uKwXCHkxl6Zy0";
        public const string TokenWithoutAlgorithm = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.ANY";

        public static readonly IDictionary<string, object> DictionaryPayload = new Dictionary<string, object>
        {
            { nameof(Customer.FirstName), Customer.FirstName },
            { nameof(Customer.Age), Customer.Age },
        };

        public static readonly string[] ServerRsaPublicKeys = { ServerRsaPublicKey1, ServerRsaPublicKey2 };

        public const string ServerRsaPublicKey1 = "-----BEGIN CERTIFICATE-----"
            + "MIICPDCCAaWgAwIBAgIBADANBgkqhkiG9w0BAQ0FADA7MQswCQYDVQQGEwJ1czEL"
            + "MAkGA1UECAwCVVMxETAPBgNVBAoMCENlcnR0ZXN0MQwwCgYDVQQDDANqd3QwHhcN"
            + "MTcwNjI3MTgzNjM3WhcNMjAwMzIzMTgzNjM3WjA7MQswCQYDVQQGEwJ1czELMAkG"
            + "A1UECAwCVVMxETAPBgNVBAoMCENlcnR0ZXN0MQwwCgYDVQQDDANqd3QwgZ8wDQYJ"
            + "KoZIhvcNAQEBBQADgY0AMIGJAoGBALsspKjcF/mB0nheaT+9KizOeBM6Qpi69LzO"
            + "LBw8rxohSFJw/BFB/ch+8jXbtq23IwtavJTwSeY6a7pbZgrwCwUK/27gy04m/tum"
            + "5FJBfCVGTTI4vqUYeTKimQzxj2pupQ+wx//1tKrXMIDGdllmQ/tffQHXxYGBR5Ol"
            + "543YRN+dAgMBAAGjUDBOMB0GA1UdDgQWBBQMfi0akrZdtPpiYSbE4h2/9vlaozAf"
            + "BgNVHSMEGDAWgBQMfi0akrZdtPpiYSbE4h2/9vlaozAMBgNVHRMEBTADAQH/MA0G"
            + "CSqGSIb3DQEBDQUAA4GBAF9pg6H7C7O5/oHeRtKSOPb9WnrHZ/mxAl30wCh2pCtJ"
            + "LkgMKKhquYmKqTA+QWpSF/Qeaq17DAf7Wq8VQ2QES9WCQm+PlBlTeZAf3UTLkxIU"
            + "DchuS8mR7QAgG67QNLl2OKMC4NWzq0d6ZYNzVqHHPe2AKgsRro6SEAv0Sf2QhE3j"
            + "-----END CERTIFICATE-----";

        public const string ServerRsaPublicKey2 = "-----BEGIN CERTIFICATE-----"
            + "MIICPDCCAaWgAwIBAgIBADANBgkqhkiG9w0BAQ0FADA7MQswCQYDVQQGEwJ1czEL"
            + "MAkGA1UECAwCVVMxETAPBgNVBAoMCENlcnR0ZXN0MQwwCgYDVQQDDANqd3QwHhcN"
            + "MTcwNjI3MTgzNjM3WhcNMjAwMzIzMTgzNjM3WjA7MQswCQYDVQQGEwJ1czELMAkG"
            + "A1UECAwCVVMxETAPBgNVBAoMCENlcnR0ZXN0MQwwCgYDVQQDDANqd3QwgZ8wDQYJ"
            + "KoZIhvcNAQEBBQADgY0AMIGJAoGBALsspKjcF/mB0nheaT+9KizOeBM6Qpi69LzO"
            + "LBw8rxohSFJw/BFB/ch+8jXbtq23IwtavJTwSeY6a7pbZgrwCwUK/27gy04m/tum"
            + "5FJBfCVGTTI4vqUYeTKimQzxj2pupQ+wx//1tKrXMIDGdllmQ/tffQHXxYGBR5Ol"
            + "543YRN+dAgMBAAGjUDBOMB0GA1UdDgQWBBQMfi0akrZdtPpiYSbE4h2/9vlaozAf"
            + "BgNVHSMEGDAWgBQMfi0akrZdtPpiYSbE4h2/9vlaozAMBgNVHRMEBTADAQH/MA0G"
            + "CSqGSIb3DQEBDQUAA4GBAF9pg6H7C7O5/oHeRtKSOPb9WnrHZ/mxAl30wCh2pCtJ"
            + "LkgMKKhquYmKqTA+QWpSF/Qeaq17DAf7Wq8VQ2QES9WCQm+PlBlTeZAf3UTLkxIU"
            + "DchuS8mR7QAgG67QNLl2OKMC4NWzq0d6ZYNzVqHHPe2AKgsRro6SEAv0Sf2QhE3j"
            + "-----END CERTIFICATE-----";

        public const string ServerRsaPrivateKey = "-----BEGIN PRIVATE KEY-----"
            + "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBALsspKjcF/mB0nhe"
            + "aT+9KizOeBM6Qpi69LzOLBw8rxohSFJw/BFB/ch+8jXbtq23IwtavJTwSeY6a7pb"
            + "ZgrwCwUK/27gy04m/tum5FJBfCVGTTI4vqUYeTKimQzxj2pupQ+wx//1tKrXMIDG"
            + "dllmQ/tffQHXxYGBR5Ol543YRN+dAgMBAAECgYEAqLIs2dA8f3Fle31ECOF6MJYK"
            + "HPJGcZcW21BK60w6WSekIkGYvgknLVxU+vvCosDLggFOtEH5qNoAnB6iUrtUgbVg"
            + "9JlZspfAgMY6uzNBrjrRtMEKWZY7MaURf8S1dkZoR6QdjMnK1/Mjno6J6bl5LBkr"
            + "wQxVeH3dllkxREOslLUCQQDfygcI7/EYjrktLQUxRI0fV0YDkntNvjDKmwh/pzQs"
            + "1sW0pj8B7d2pwdOKy+P5p7ubXlXtom2iZZXNWQq/mranAkEA1h13/ZaDSh85YLcH"
            + "y+MJ/ij5treXdEM5E2uXgMoMBJJhClaMHloFoPIS4Q5Rk1gkI6kWZvA7Ldp2X28x"
            + "Mz8EGwJAKJSN6gT4hyd6VMLRKjnwDTraK1OooFRYrKSoSd2cDHV1rGhpDISBqYLI"
            + "RWbrlB3iWy4kDs9hag1ZuL7owA3iCQJAQqEE9+rgjC5PQqNyT6YlM+w4WP2kqc9J"
            + "cZunl7JILxwGCpuIGuHUopLyAQrdo8Zn6JjzmbDkGY7EC0qkute/RQJAEKofKVOq"
            + "YouEKIpRSnxeRi45LX0ouRgTmwT/8xr6VxVKe1eToMJ85f/7FXewWR+W1dIN+NXi"
            + "MzhzkzbNqAeDkg=="
            + "-----END PRIVATE KEY-----";

        public static readonly IJwtAlgorithm HMACSHA256Algorithm = new HMACSHA256Algorithm();

        public static readonly IJwtAlgorithm RS256Algorithm = new RS256Algorithm(
            new X509Certificate2(
                Encoding.ASCII.GetBytes(ServerRsaPublicKey1)));
    }
}