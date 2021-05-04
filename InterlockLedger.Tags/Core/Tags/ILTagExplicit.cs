// ******************************************************************************************************************************
//
// Copyright (c) 2018-2021 InterlockLedger Network
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met
//
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES, LOSS OF USE, DATA, OR PROFITS, OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// ******************************************************************************************************************************

using System;
using System.IO;
using System.Text.Json.Serialization;

namespace InterlockLedger.Tags
{
    public abstract class ILTagExplicit<T> : ILTagOfExplicit<T>, IMemoryBackedTag
    {
        [JsonIgnore]
        public byte[] EncodedBytes => _encodedBytes.Value;

        public sealed override Stream OpenReadingStream() => new MemoryStream(EncodedBytes, writable: false);

        protected ILTagExplicit(ulong tagId, T value) : base(tagId, value) {
        }

        protected ILTagExplicit(ulong alreadyDeserializedTagId, Stream s, Action<ITag> setup = null)
                : base(alreadyDeserializedTagId, s, setup) {
        }

        protected sealed override bool NonFullBytes => false;

        protected static T FromBytesHelper(byte[] bytes, Func<Stream, T> deserialize) {
            if (bytes == null || bytes.Length == 0)
                return default;
            if (deserialize == null) {
                throw new ArgumentNullException(nameof(deserialize));
            }
            using var s = new MemoryStream(bytes);
            return deserialize(s);
        }

        protected sealed override T DeserializeValueFromStream(StreamSpan s)
            => FromBytes(s.ReadAllBytesAsync().Result);

        protected abstract T FromBytes(byte[] bytes);

        protected sealed override void SerializeValueToStream(Stream s, T value) => s.WriteBytes(ToBytes(value));

        protected abstract byte[] ToBytes(T Value);

        protected override ulong ValueEncodedLength(T value) => (ulong)(ToBytes(value)?.Length ?? 0);
    }
}