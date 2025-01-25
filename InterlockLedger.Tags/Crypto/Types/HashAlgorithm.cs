// ******************************************************************************************************************************
//  
// Copyright (c) 2018-2025 InterlockLedger Network
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

using System.Security.Cryptography;

namespace InterlockLedger.Tags;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HashAlgorithm : ushort
{
    SHA1 = 0,
    SHA256 = 1,
    SHA384 = 5,
    SHA512 = 2,
    SHA3_256 = 3,
    SHA3_384 = 6,
    SHA3_512 = 4,

    Invalid = 0xFFFE,
    Copy = 0xFFFF
}

public static class HashAlgorithmExtensions
{
    public static HashAlgorithmName ToName(this HashAlgorithm value) => value switch {
        HashAlgorithm.SHA1 => HashAlgorithmName.SHA1,
        HashAlgorithm.SHA256 => HashAlgorithmName.SHA256,
        HashAlgorithm.SHA384 => HashAlgorithmName.SHA384,
        HashAlgorithm.SHA512 => HashAlgorithmName.SHA512,
        HashAlgorithm.SHA3_256 => HashAlgorithmName.SHA3_256,
        HashAlgorithm.SHA3_384 => HashAlgorithmName.SHA3_384,
        HashAlgorithm.SHA3_512 => HashAlgorithmName.SHA3_512,
        _ => throw new NotSupportedException()
    };
}