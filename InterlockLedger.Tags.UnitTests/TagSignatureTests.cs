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

namespace InterlockLedger.Tags;
[TestFixture]
[SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "Buggy warning")]
public class TagSignatureTests
{
    [TestCase(new byte[] { 38, 4, 4, 0, 0, 0 }, Algorithm.EcDSA, new byte[] { 0, 0 })]
    [TestCase(new byte[] { 38, 4, 0, 0, 0, 0 }, Algorithm.RSA, new byte[] { 0, 0 })]
    [TestCase(new byte[] { 38, 2, 3, 0 }, Algorithm.ElGamal, new byte[] { })]
    public void NewTagSignatureFromStream(byte[] bytes, Algorithm algorithm, byte[] data) {
        using var ms = new MemoryStream(bytes);
        var tag = ms.Decode<TagSignature>();
        Assert.That(tag, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(tag.TagId, Is.EqualTo(ILTagId.Signature));
            Assert.That(tag.Algorithm, Is.EqualTo(algorithm));
            Assert.That(tag.Data?.Length ?? 0, Is.EqualTo(data.Length));
        });
    }

    [TestCase(Algorithm.EcDSA, new byte[] { 0, 0 }, ExpectedResult = new byte[] { 38, 4, 4, 0, 0, 0 })]
    [TestCase(Algorithm.RSA, new byte[] { 0, 0 }, ExpectedResult = new byte[] { 38, 4, 0, 0, 0, 0 })]
    [TestCase(Algorithm.ElGamal, new byte[] { }, ExpectedResult = new byte[] { 38, 2, 3, 0 })]
    public byte[] SerializeTagSignature(Algorithm algorithm, byte[] data) => new TagSignature(algorithm, data).EncodedBytes();
}