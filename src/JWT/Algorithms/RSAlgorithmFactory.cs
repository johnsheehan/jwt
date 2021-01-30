using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace JWT.Algorithms
{
    /// <inheritdoc />
    public sealed class RSAlgorithmFactory : HMACSHAAlgorithmFactory
    {
        private readonly Func<X509Certificate2> _certFactory;
        private readonly RSA _publicKey;
        private readonly RSA _privateKey;

        /// <summary>
        /// Creates an instance of the <see cref="RSAlgorithmFactory" /> class using the provided <see cref="X509Certificate2" />.
        /// </summary>
        /// <param name="certFactory">Func that returns <see cref="X509Certificate2" /> which will be used to instantiate <see cref="RS256Algorithm" /></param>
        public RSAlgorithmFactory(Func<X509Certificate2> certFactory)
        {
            _certFactory = certFactory ?? throw new ArgumentNullException(nameof(certFactory));
        }

        /// <summary>
        /// Creates an instance of <see cref="RSAlgorithmFactory"/> using the provided public key only.
        /// </summary>
        /// <param name="publicKey">The public key for verifying the data.</param>
        public RSAlgorithmFactory(RSA publicKey)
        {
            _publicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
        }

        /// <summary>
        /// Creates an instance of <see cref="RSAlgorithmFactory"/> using the provided pair of public and private keys.
        /// </summary>
        /// <param name="publicKey">The public key for verifying the data.</param>
        /// <param name="privateKey">The private key for signing the data.</param>
        public RSAlgorithmFactory(RSA publicKey, RSA privateKey)
            : this(publicKey)
        {
            _privateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
        }

        protected override IJwtAlgorithm Create(JwtAlgorithmName algorithm)
        {
            switch (algorithm)
            {
                case JwtAlgorithmName.RS256:
                    return CreateRS256Algorithm();
                case JwtAlgorithmName.RS384:
                    return CreateRS384Algorithm();
                case JwtAlgorithmName.RS512:
                    return CreateRS512Algorithm();
                default:
                    throw new NotSupportedException($"For algorithm {Enum.GetName(typeof(JwtAlgorithmName), algorithm)} please use the appropriate factory by implementing {nameof(IAlgorithmFactory)}");
            }
        }

        private RS256Algorithm CreateRS256Algorithm()
        {
            if (_certFactory is object)
            {
                return new RS256Algorithm(_certFactory());
            }
            if (_publicKey is object && _privateKey is object)
            {
                return new RS256Algorithm(_publicKey, _privateKey);
            }
            if (_publicKey is object)
            {
                return new RS256Algorithm(_publicKey);
            }

            throw new InvalidOperationException("Can't create a new algorithm without a certificate factory, private key or public key");
        }

        private RS384Algorithm CreateRS384Algorithm()
        {
            if (_certFactory is object)
            {
                return new RS384Algorithm(_certFactory());
            }
            if (_publicKey is object && _privateKey is object)
            {
                return new RS384Algorithm(_publicKey, _privateKey);
            }
            if (_publicKey is object)
            {
                return new RS384Algorithm(_publicKey);
            }

            throw new InvalidOperationException("Can't create a new algorithm without a certificate factory, private key or public key");
        }

        private RS512Algorithm CreateRS512Algorithm()
        {
            if (_certFactory is object)
            {
                return new RS512Algorithm(_certFactory());
            }
            if (_publicKey is object && _privateKey is object)
            {
                return new RS512Algorithm(_publicKey, _privateKey);
            }
            if (_publicKey is object)
            {
                return new RS512Algorithm(_publicKey);
            }

            throw new InvalidOperationException("Can't create a new algorithm without a certificate factory, private key or public key");
        }
    }
}
