﻿using System.Collections.Generic;

namespace JWT.Tests.Common
{
    public static class TestData
    {
        public static readonly Customer Customer = new Customer { FirstName = "Bob", Age = 37 };

        public const string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        public const string MalformedToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        public const string ExtraHeadersToken = "eyJmb28iOiJiYXIiLCJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.slrbXF9VSrlX7LKsV-Umb_zEzWLxQjCfUOjNTbvyr1g";

        // Security Constants
        public const string AlgorithmNoneToken = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.ANY";
        

        public static readonly IDictionary<string, object> DictionaryPayload = new Dictionary<string, object>
        {
            { "FirstName", "Bob" },
            { "Age", 37 }
        };

        public const string ServerRSAPublicKey = "-----BEGIN CERTIFICATE-----"
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

        public const string ServerRSAPrivateKey = "-----BEGIN PRIVATE KEY-----"
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

    }
}
