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
    public abstract class ImplicitLengthTag<T> : ILTag
    {
        [JsonIgnore]
        public override object AsJson => Value;

        public T Value { get; set; }

        public sealed override Stream SerializeInto(Stream s) {
            if (s is null) return s;
            try {
                s.ILIntEncode(TagId);
                SerializeInner(s, Value);
            } finally {
                s.Flush();
            }
            return s;
        }

        public override bool ValueIs<TV>(out TV value) {
            if (Value is TV tvalue) {
                value = tvalue;
                return true;
            }
            value = default;
            return false;
        }

        //protected ILTagImplicit() : base(0) {
        //}

        protected ImplicitLengthTag(ulong tagId, T value) : base(tagId) {
            Value = value;
        }

        protected ImplicitLengthTag(Stream s, ulong alreadyDeserializedTagId, Action<ILTag> setup) : base(alreadyDeserializedTagId) {
            setup?.Invoke(this);
            Value = DeserializeInner(s);
        }

        protected ImplicitLengthTag(Stream s, ulong alreadyDeserializedTagId) : this(s, alreadyDeserializedTagId, setup: null) {
        }

        protected ImplicitLengthTag(ulong tagId, Stream s) : this(s, tagId, setup: t => TagProvider.ValidateTagId(t, s)) {
        }

        protected abstract T DeserializeInner(Stream s);

        protected abstract void SerializeInner(Stream s, T value);
    }
}