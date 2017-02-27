using System.Collections.Generic;
using System.Text;

namespace JWT
{
    public sealed class JwtEncoder : IJwtEncoder
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _jsonSerializer;

        public JwtEncoder(IJwtAlgorithm algorithm, IJsonSerializer jsonSerializer)
        {
            _algorithm = algorithm;
            _jsonSerializer = jsonSerializer;
        }

        /// <inheritdoc />
        public string Encode(object payload, string key)
        {
            return Encode(null, payload, Encoding.UTF8.GetBytes(key));
        }

        /// <inheritdoc />
        public string Encode(object payload, byte[] key)
        {
            return Encode(null, payload, key);
        }

        /// <inheritdoc />
        public string Encode(IDictionary<string, object> extraHeaders, object payload, string key)
        {
            return Encode(extraHeaders, payload, Encoding.UTF8.GetBytes(key));
        }

        /// <inheritdoc />
        public string Encode(IDictionary<string, object> extraHeaders, object payload, byte[] key)
        {
            var segments = new List<string>(3);

            var header = extraHeaders != null ? new Dictionary<string, object>(extraHeaders) : new Dictionary<string, object>();
            header.Add("typ", "JWT");
            header.Add("alg", _algorithm.Name);

            var headerBytes = Encoding.UTF8.GetBytes(_jsonSerializer.Serialize(header));
            var payloadBytes = Encoding.UTF8.GetBytes(_jsonSerializer.Serialize(payload));

            segments.Add(JsonWebToken.Base64UrlEncode(headerBytes));
            segments.Add(JsonWebToken.Base64UrlEncode(payloadBytes));

            var stringToSign = string.Join(".", segments.ToArray());
            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            var signature = _algorithm.Sign(key, bytesToSign);
            segments.Add(JsonWebToken.Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }
    }
}