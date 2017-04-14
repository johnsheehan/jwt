﻿using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace JWT.Algorithms
{
    public sealed class RS256Algorithm : IJwtAlgorithm
    {
        private readonly X509Certificate2 _cert;

        public RS256Algorithm(X509Certificate2 cert)
        {
            _cert = cert;
        }

        public byte[] Sign(byte[] key, byte[] bytesToSign)
        {
#if NETSTANDARD1_3
            var rsa = (RSACryptoServiceProvider)_cert.GetRSAPrivateKey();
#else
            var rsa = (RSACryptoServiceProvider)_cert.PrivateKey;
#endif
            var param = new CspParameters
            {
                KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName,
                KeyNumber = rsa.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2
            };
            var csp = new RSACryptoServiceProvider(param) { PersistKeyInCsp = false };
            return csp.SignData(bytesToSign, "SHA256");
        }

        public string Name => JwtHashAlgorithm.RS256.ToString();
    }
}