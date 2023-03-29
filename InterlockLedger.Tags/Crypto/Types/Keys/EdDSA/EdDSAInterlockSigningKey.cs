// ******************************************************************************************************************************
//  
// Copyright (c) 2018-2023 InterlockLedger Network
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

namespace InterlockLedger.Tags;

public class EdDSAInterlockSigningKey : InterlockSigningKey
{
    private readonly TagEdDSAParameters _keyParameters;

    public override byte[] AsSessionState {
        get {
            using var ms = new MemoryStream();
            _ = ms.EncodeTag(_data).EncodeTag(_keyParameters);
            return ms.ToArray();
        }
    }
    public static new EdDSAInterlockSigningKey FromSessionState(byte[] bytes) {
        using var s = new MemoryStream(bytes);
        return new EdDSAInterlockSigningKey(s.Decode<InterlockSigningKeyData>(), s.Decode<TagEdDSAParameters>());
    }


    public EdDSAInterlockSigningKey(InterlockSigningKeyData data, byte[] decrypted) : base(data) {
        if (data.Required().EncryptedContentType != 0)
            throw new ArgumentException($"Wrong kind of EncryptedContentType {data.EncryptedContentType}");
        using var s = new MemoryStream(decrypted);
        _keyParameters = s.Decode<TagEdDSAParameters>().Validate();
    }

    public EdDSAInterlockSigningKey(InterlockSigningKeyData? tag, TagEdDSAParameters? parameters)
        : base(tag.Required()) => _keyParameters = parameters.Validate();


    public override TagSignature Sign(byte[] data) => new(Algorithm.EdDSA, EdDSAHelper.HashAndSign(data, _keyParameters.Value));

    public override TagSignature Sign<T>(T data) => new(Algorithm.EdDSA, EdDSAHelper.HashAndSignBytes(data, _keyParameters.Value));

}
