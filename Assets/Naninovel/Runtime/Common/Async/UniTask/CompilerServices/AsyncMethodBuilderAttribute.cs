// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

#pragma warning disable CS0436
#pragma warning disable CS0436

namespace System.Runtime.CompilerServices
{
    internal sealed class AsyncMethodBuilderAttribute : Attribute
    {
        public Type BuilderType { get; }

        public AsyncMethodBuilderAttribute (Type builderType)
        {
            BuilderType = builderType;
        }
    }
}
