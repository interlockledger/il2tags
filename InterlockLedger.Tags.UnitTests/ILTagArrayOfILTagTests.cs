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
public class ILTagArrayOfILTagTests
{
    [TestCase((byte[])null, new byte[] { 21, 0 }, TestName = "DecodeArray_a_Null_as_Null")]
    [TestCase(new byte[0], new byte[] { 21, 1, 0 }, TestName = "DecodeArray_an_Empty_Array")]
    [TestCase(new byte[] { 1, 2, 3 }, new byte[] { 21, 10, 3, 0, 1, 1, 0, 1, 2, 0, 1, 3 }, TestName = "DecodeArray_One_Array_with_Four_Explicitly_Tagged_Bytes")]
    public void DecodeArray(byte[] bytes, byte[] encodedBytes) {
        using var ms = new MemoryStream(encodedBytes);
        var value = ms.DecodeArray<byte, TestTagOfOneByte>(s => new TestTagOfOneByte(s.DecodeTagId(), s));
        Assert.That(value, Is.EqualTo(bytes));
    }


    [TestCase(new byte[0], new byte[0], new byte[] { 21, 1, 0 }, TestName = "Deserialize_an_Empty_Array")]
    [TestCase(new byte[0], new byte[] { 0 }, new byte[] { 21, 3, 1, 16, 0 }, TestName = "Deserialize_One_Array_with_Zero_Bytes")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 4 }, new byte[] { 21, 7, 1, 16, 4, 1, 2, 3, 2 }, TestName = "Deserialize_One_Array_with_Four_Bytes")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 2, 4 }, new byte[] { 21, 9, 2, 16, 2, 1, 2, 16, 2, 3, 2 }, TestName = "Deserialize_Two_Arrays_with_Two_Bytes")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 3, 4 }, new byte[] { 21, 9, 2, 16, 3, 1, 2, 3, 16, 1, 2 }, TestName = "Deserialize_Two_Arrays_with_one_and_Three_Bytes")]
    public void DeserializeILTagILTagArray(byte[] bytes, byte[] splits, byte[] encodedBytes) {
        using var ms = new MemoryStream(encodedBytes);
        var value = ms.DecodeTagArray<ILTagByteArray>();
        var array = BuildArrayOfArrays(bytes, splits);
        CompareArrays<ILTagByteArray, byte[]>(array, value);
    }

    [TestCase(new byte[0], new byte[0], new byte[] { 21, 1, 0 }, TestName = "Deserialize_an_Empty_Array_Generic")]
    [TestCase(new byte[0], new byte[] { 0 }, new byte[] { 21, 3, 1, 16, 0 }, TestName = "Deserialize_One_Array_with_Zero_Bytes_Generic")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 4 }, new byte[] { 21, 7, 1, 16, 4, 1, 2, 3, 2 }, TestName = "Deserialize_One_Array_with_Four_Bytes_Generic")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 2, 4 }, new byte[] { 21, 9, 2, 16, 2, 1, 2, 16, 2, 3, 2 }, TestName = "Deserialize_Two_Arrays_with_Two_Bytes_Generic")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 3, 4 }, new byte[] { 21, 9, 2, 16, 3, 1, 2, 3, 16, 1, 2 }, TestName = "Deserialize_Two_Arrays_with_one_and_Three_Bytes_Generic")]
    public void DeserializeILTagILTagArrayGeneric(byte[] bytes, byte[] splits, byte[] encodedBytes) {
        using var ms = new MemoryStream(encodedBytes);
        var tagValue = ms.DecodeTag();
        Assert.That(tagValue, Is.InstanceOf<ILTagArrayOfILTag<ILTag>>());
        var value = ((ILTagArrayOfILTag<ILTag>)tagValue).Value;
        var array = BuildArrayOfArrays(bytes, splits);
        CompareArrays<ILTagByteArray, byte[]>(array, value);
    }

    [Test]
    public void GuaranteeBijectiveBehaviorEmptyArray()
        => GuaranteeBijectiveBehavior([]);

    [Test]
    public void GuaranteeBijectiveBehaviorFourElementsArray()
        => GuaranteeBijectiveBehavior([ILTagBool.False, ILTagBool.True, ILTagBool.True, ILTagBool.True]);

    [Test]
    public void GuaranteeBijectiveBehaviorTwoElementsArray()
        => GuaranteeBijectiveBehavior([ILTagBool.False, ILTagBool.True]);

    [TestCase((byte[])null, new byte[0], ExpectedResult = new byte[] { 21, 0 }, TestName = "Serialize_a_Null_Array")]
    [TestCase(new byte[0], new byte[0], ExpectedResult = new byte[] { 21, 1, 0 }, TestName = "Serialize_an_Empty_Array")]
    [TestCase(new byte[0], new byte[] { 0 }, ExpectedResult = new byte[] { 21, 3, 1, 16, 0 }, TestName = "Serialize_One_Array_with_One_Byte")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 4 }, ExpectedResult = new byte[] { 21, 7, 1, 16, 4, 1, 2, 3, 2 }, TestName = "Serialize_One_Array_with_Four_Bytes")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 2, 4 }, ExpectedResult = new byte[] { 21, 9, 2, 16, 2, 1, 2, 16, 2, 3, 2 }, TestName = "Serialize_Two_Arrays_with_Two_Bytes")]
    [TestCase(new byte[] { 1, 2, 3, 2 }, new byte[] { 3, 4 }, ExpectedResult = new byte[] { 21, 9, 2, 16, 3, 1, 2, 3, 16, 1, 2 }, TestName = "Serialize_Two_Arrays_with_One_and_Three_Bytes")]
    public byte[] SerializeILTagILTagArray(byte[] bytes, byte[] splits) {
        byte[] encodedBytes = new ILTagArrayOfILTag<ILTagByteArray>(BuildArrayOfArrays(bytes, splits)).EncodedBytes();
        TestContext.Out.WriteLine(encodedBytes.AsLiteral());
        return encodedBytes;
    }

    private static ILTagByteArray[] BuildArrayOfArrays(byte[] bytes, byte[] splits) {
        if (bytes == null)
            return null;
        var list = new List<ILTagByteArray>();
        if ((splits?.Length ?? 0) > 0) {
            var lastSplit = 0;
            foreach (var split in splits) {
                var length = split - lastSplit;
                var partialBytes = new ReadOnlyMemory<byte>(bytes, lastSplit, length);
                list.Add(new ILTagByteArray(partialBytes));
                lastSplit = split;
            }
        }
        return [.. list];
    }

    private static void CompareArrays<T, TT>(T[] array, ILTag[] value) where T : ILTagOf<TT> {
        if (array == null)
            Assert.That(value, Is.Null);
        else {
            Assert.That(value, Has.Length.EqualTo(array.Length));
            for (var i = 0; i < array.Length; i++) {
                var arrayValue = array[i].Value;
                var valueValue = ((T)value[i]).Value;
                Assert.That(valueValue, Is.EqualTo(arrayValue));
            }
        }
    }

    private static void GuaranteeBijectiveBehavior(ILTagBool[] array) {
        var ilarray = new ILTagArrayOfILTag<ILTagBool>(array);
        var encodedBytes = ilarray.EncodedBytes();
        using var ms = new MemoryStream(encodedBytes);
        var value = ms.DecodeTagArray<ILTagBool>();
        CompareArrays<ILTagBool, bool>(array, value);
        var newEncodedBytes = new ILTagArrayOfILTag<ILTagBool>(value).EncodedBytes();
        Assert.That(newEncodedBytes, Is.EqualTo(encodedBytes));
    }

    private class TestTagOfOneByte(ulong tagId, Stream s) : ILTagOfExplicit<byte>(tagId, s)
    {
        protected override Task<byte> ValueFromStreamAsync(WrappedReadonlyStream s) => Task.FromResult(s.ReadSingleByte());
        protected override Task<Stream> ValueToStreamAsync(Stream s) {
            s.WriteByte(Value);
            return Task.FromResult(s);
        }
    }
}