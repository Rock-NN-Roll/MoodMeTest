// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.


namespace Naninovel
{
    public readonly struct RawDataRepresentation
    {
        public readonly string Extension, MimeType;

        public RawDataRepresentation (string extension, string mimeType)
        {
            Extension = extension;
            MimeType = mimeType;
        }
    }

    /// <summary>
    /// Implementation is able to convert <see cref="T:byte[]"/> to <typeparamref name="TResult"/>
    /// and provide additional information about the raw data representation of the object. 
    /// </summary>
    public interface IRawConverter<TResult> : IConverter<byte[], TResult>
    {
        RawDataRepresentation[] Representations { get; }
    }
}
