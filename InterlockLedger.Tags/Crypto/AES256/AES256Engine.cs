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
using System.Security.Cryptography;

namespace InterlockLedger.Tags
{
    public class AES256Engine : ISymmetricEngine
    {
        public byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
            => key is null
                ? throw new ArgumentNullException(nameof(key))
                : Decrypt(cipherData.Required(nameof(cipherData)), readHeader: _ => (key, iv));

        public byte[] Decrypt(byte[] cipherData, Func<MemoryStream, (byte[] key, byte[] iv)> readHeader) {
            if (cipherData is null)
                throw new ArgumentNullException(nameof(cipherData));
            if (readHeader is null)
                throw new ArgumentNullException(nameof(readHeader));
            using var source = new MemoryStream(cipherData);
            (byte[] key, byte[] iv) = readHeader(source);
            using var algorithm = new RijndaelManaged {
                KeySize = 256,
                BlockSize = 128,
                IV = iv,
                Key = key,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };
            using var cs = new CryptoStream(source, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            using var dest = new MemoryStream();
            cs.CopyTo(dest);
            return dest.ToArray();
        }

        public (byte[] cipherData, byte[] key, byte[] iv) Encrypt(byte[] clearData,
                                                                  Action<MemoryStream, byte[], byte[]> writeHeader = null,
                                                                  byte[] key = null,
                                                                  byte[] iv = null) {
            using var algorithm = BuildAlgorithm(key, iv);
            if (clearData is null)
                throw new ArgumentNullException(nameof(clearData));
            using var ms = new MemoryStream();
            writeHeader?.Invoke(ms, algorithm.Key, algorithm.IV);
            using (var cs = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write)) {
                cs.Write(clearData, 0, clearData.Length);
                cs.Close();
            }
            return (ms.ToArray(), algorithm.Key, algorithm.IV);
        }
        public (byte[] cipherData, byte[] key, byte[] iv) Encrypt(Stream clearDataStream,
                                                                  Action<MemoryStream, byte[], byte[]> writeHeader = null,
                                                                  byte[] key = null,
                                                                  byte[] iv = null) {
            using var algorithm = BuildAlgorithm(key, iv);
            if (clearDataStream is null)
                throw new ArgumentNullException(nameof(clearDataStream));
            using var ms = new MemoryStream();
            writeHeader?.Invoke(ms, algorithm.Key, algorithm.IV);
            using (var cs = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write)) {
                clearDataStream.CopyTo(cs);
                cs.Close();
            }
            return (ms.ToArray(), algorithm.Key, algorithm.IV);
        }

        private static SymmetricAlgorithm BuildAlgorithm(byte[] key, byte[] iv) {
            var AES = new RijndaelManaged {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };
            if (iv == null || iv.Length != 16)
                AES.GenerateIV();
            else
                AES.IV = iv;
            if (key == null || key.Length != 32)
                AES.GenerateKey();
            else
                AES.Key = key;
            return AES;
        }
    }
}