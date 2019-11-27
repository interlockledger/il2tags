/******************************************************************************************************************************

Copyright (c) 2018-2019 InterlockLedger Network
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

* Neither the name of the copyright holder nor the names of its
  contributors may be used to endorse or promote products derived from
  this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

******************************************************************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InterlockLedger.Tags
{
    public class ILTagDictionary<T> : ILTagAbstractDictionary<T> where T : ILTag
    {
        public ILTagDictionary(params (string key, T value)[] pairs) : this(pairs.ToDictionary(p => p.key, pp => pp.value)) {
        }

        public ILTagDictionary(object opaqueValue) : this(Elicit(opaqueValue)) {
        }

        public ILTagDictionary(Dictionary<string, T> value) : base(ILTagId.Dictionary, value) {
        }

        public override object AsJson => Value.ToDictionary(p => p.Key, pp => new { pp.Value.TagId, Value = pp.Value.AsJson });

        internal ILTagDictionary(Stream s) : base(ILTagId.Dictionary, s) {
        }

        protected override T DecodeValue(Stream s) => s.Decode<T>();

        protected override void EncodeValue(Stream s, T value) => s.EncodeTag(value);

        private static Dictionary<string, T> Elicit(object opaqueValue) {
            if (opaqueValue is Dictionary<string, T> dict)
                return dict;
            if (opaqueValue is Dictionary<string, object> odict)
                return odict.ToDictionary(p => p.Key, pp => (T)pp.Value.AsNavigable());
            return new Dictionary<string, T>();
        }
    }
}