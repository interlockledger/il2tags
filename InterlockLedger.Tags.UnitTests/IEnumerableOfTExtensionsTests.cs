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
public class IEnumerableOfTExtensionsTests
{
    [Test]
    public void AppendedOf() =>
        Assert.Multiple(() => {
            Assert.That(() => ((object)null).AppendedOf(null), Is.EquivalentTo(new object[] { null }), "Null concat Null should be equal to [Null]");
            Assert.That(() => ((object)null).AppendedOf(Enumerable.Empty<object>()), Is.EquivalentTo(new object[] { null }), "Null concat Empty should be equal to [Null]");
            Assert.That(() => ((string)null).AppendedOf(["A"]), Is.EquivalentTo(new string[] { null, "A" }), "Null concat [A] should be equal to [Null, A]");
            Assert.That(() => 1.AppendedOf(null), Is.EquivalentTo(new int[] { 1 }), "1 concat Null should be equal to [1]");
            Assert.That(() => 1.AppendedOf((IEnumerable<int>)[2, 3]), Is.EquivalentTo(new int[] { 1, 2, 3 }), "1 concat [2,3] should be equal to [1, 2, 3]");
        });

    [Test]
    public void AppendedOfParams() =>
        Assert.Multiple(() => {
            Assert.That(() => ((object)null).AppendedOf(), Is.EquivalentTo(new object[] { null }), "Null concat Empty should be equal to [Null]");
            Assert.That(() => ((string)null).AppendedOf("A"), Is.EquivalentTo(new string[] { null, "A" }), "Null concat [A] should be equal to [Null, A]");
            Assert.That(() => 1.AppendedOf(), Is.EquivalentTo(new int[] { 1 }), "1 concat Empty should be equal to [1]");
            Assert.That(() => 1.AppendedOf(2, 3), Is.EquivalentTo(new int[] { 1, 2, 3 }), "1 concat [2,3] should be equal to [1, 2, 3]");
        });

    [Test]
    public void SafeConcat() =>
        Assert.Multiple(() => {
            Assert.That(() => ((IEnumerable<object>)null).SafeConcat([]), Is.Empty, "Null concat Empty should be equal to Empty");
            Assert.That(() => ((IEnumerable<object>)null).SafeConcat(null), Is.Empty, "Null concat Null should be equal to Empty");
            Assert.That(() => Enumerable.Empty<object>().SafeConcat(null), Is.Empty, "Empty concat Null should be equal to Empty");
            Assert.That(() => Enumerable.Empty<object>().SafeConcat([]), Is.Empty, "Empty concat Empty should be equal to Empty");
            Assert.That(() => ((IEnumerable<int>)null).SafeConcat([1]), Is.EquivalentTo(new int[] { 1 }), "Null concat [1] should be equal to [1]");
            Assert.That(() => new int[] { 1 }.SafeConcat([2, 3]), Is.EquivalentTo(new int[] { 1, 2, 3 }), "[1] concat [2,3] should be equal to [1, 2, 3]");
        });
}