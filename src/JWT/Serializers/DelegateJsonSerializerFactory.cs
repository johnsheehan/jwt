using System;

﻿namespace JWT.Serializers
{
    internal sealed class DelegateJsonSerializerFactory : IJsonSerializerFactory
    {
        private readonly Func<IJsonSerializer> _factory;

        public DelegateJsonSerializerFactory(IJsonSerializer jsonSerializer) :
            this(() => jsonSerializer)
        {
            if (jsonSerializer is null)
                throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public DelegateJsonSerializerFactory(IJsonSerializerFactory factory) :
            this(() => factory?.Create())
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
        }

        public DelegateJsonSerializerFactory(Func<IJsonSerializer> factory) =>
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

        public IJsonSerializer Create() =>
            _factory();
    }
}
