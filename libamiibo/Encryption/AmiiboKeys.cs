/*
 * Copyright (C) 2015 Marcos Vives Del Sol
 * Copyright (C) 2016 Benjamin Krämer
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using LibAmiibo.Helper;
using System.Security.Cryptography;

namespace LibAmiibo.Encryption
{
    public class AmiiboKeys
    {
        public const int HMAC_POS_DATA = 0x008;
        public const int HMAC_POS_TAG = 0x1B4;

        private KeygenMasterkeys data;
        private KeygenMasterkeys tag;

        internal static AmiiboKeys Unserialize(BinaryReader reader)
        {
            return new AmiiboKeys
            {
                data = KeygenMasterkeys.Unserialize(reader),
                tag = KeygenMasterkeys.Unserialize(reader),
            };
        }

        internal void Serialize(BinaryWriter writer)
        {
            this.data.Serialize(writer);
            this.tag.Serialize(writer);
        }

        public static AmiiboKeys LoadKeys()
        {
            return Unserialize(new BinaryReader(new MemoryStream(KeyTables.RETAIL)));
        }

        public bool Unpack(byte[] tag, byte[] plain)
        {
            byte[] internalBytes = NtagHelpers.GetInternalTag(tag);

            // Generate keys
            KeygenDerivedkeys dataKeys = GenerateKey(this.data, internalBytes);
            KeygenDerivedkeys tagKeys = GenerateKey(this.tag, internalBytes);

            // Decrypt
            dataKeys.Cipher(internalBytes, plain, false);

            // Init OpenSSL HMAC context
            var hmac = new HMACSHA256(tagKeys.hmacKey);

            // Regenerate tag HMAC. Note: order matters, data HMAC depends on tag HMAC!
            var hmdata = hmac.ComputeHash(plain, 0x1D4, 0x34);
            Array.Copy(hmdata, 0, plain, HMAC_POS_TAG, hmdata.Length);

            // Regenerate data HMAC
            hmac = new HMACSHA256(dataKeys.hmacKey);
            hmdata = hmac.ComputeHash(plain, 0x029, 0x1DF);
            Array.Copy(hmdata, 0, plain, HMAC_POS_DATA, hmdata.Length);

            return
                NativeHelpers.MemCmp(plain, internalBytes, HMAC_POS_DATA, 32) &&
                NativeHelpers.MemCmp(plain, internalBytes, HMAC_POS_TAG, 32);
        }

        public void Pack(byte[] plain, byte[] tag)
        {
            byte[] cipher = new byte[NtagHelpers.NFC3D_AMIIBO_SIZE];

            // Generate keys
            var tagKeys = GenerateKey(this.tag, plain);
            var dataKeys = GenerateKey(this.data, plain);

            // Init OpenSSL HMAC context
            var hmac = new HMACSHA256(tagKeys.hmacKey);

            // Generate tag HMAC
            var hmdata = hmac.ComputeHash(plain, 0x1D4, 0x34);
            Array.Copy(hmdata, 0, cipher, HMAC_POS_TAG, hmdata.Length);

            // Generate data HMAC
            hmac = new HMACSHA256(dataKeys.hmacKey);
            hmac.TransformBlock(plain, 0x029, 0x18B, null, 0);        // Data
            hmac.TransformBlock(cipher, HMAC_POS_TAG, 0x20, null, 0); // Tag HMAC
            hmac.TransformFinalBlock(plain, 0x1D4, 0x34);             // Tag
            Array.Copy(hmac.Hash, 0, cipher, HMAC_POS_DATA, hmac.Hash.Length);

            // Encrypt
            dataKeys.Cipher(plain, cipher, true);

            // Convert back to hardware
            NtagHelpers.InternalToTag(cipher, tag);
        }

        private static byte[] CalcSeed(byte[] dump)
        {
            byte[] key = new byte[KeygenMasterkeys.NFC3D_KEYGEN_SEED_SIZE];
            Array.Copy(dump, 0x029, key, 0x00, 0x02);
            Array.Copy(dump, 0x1D4, key, 0x10, 0x08);
            Array.Copy(dump, 0x1D4, key, 0x18, 0x08);
            Array.Copy(dump, 0x1E8, key, 0x20, 0x20);
            return key;
        }

        private static KeygenDerivedkeys GenerateKey(KeygenMasterkeys masterKeys, byte[] dump)
        {
            byte[] seed = CalcSeed(dump);
            return masterKeys.GenerateKey(seed);
        }
    }
}