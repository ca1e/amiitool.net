// The MIT License (MIT)

// Copyright (c) 2020 Hans Wolff

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace LibAmiibo.Helper;

// see: https://gist.github.com/hanswolff/8809275
public class AesCounterMode : SymmetricAlgorithm
{
    private readonly byte[] _noncecounter;
    private readonly AesManaged _aes;

    public AesCounterMode(byte[] ivs)
    {
        _aes = new AesManaged
        {
            Mode = CipherMode.ECB,
            Padding = PaddingMode.None
        };

        _noncecounter = ivs;
    }

    private static ulong ConvertNonce(byte[] nonce)
    {
        if (nonce == null) throw new ArgumentNullException(nameof(nonce));
        if (nonce.Length < sizeof(ulong)) throw new ArgumentException($"{nameof(nonce)} must have at least {sizeof(ulong)} bytes");

        return BitConverter.ToUInt64(nonce);
    }

    public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] ignoredParameter)
    {
        return new CounterModeCryptoTransform(_aes, rgbKey, _noncecounter);
    }

    public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] ignoredParameter)
    {
        return new CounterModeCryptoTransform(_aes, rgbKey, _noncecounter);
    }

    public override void GenerateKey()
    {
        _aes.GenerateKey();
    }

    public override void GenerateIV()
    {
        // IV not needed in Counter Mode
    }
}

public class CounterModeCryptoTransform : ICryptoTransform
{
    private readonly byte[] _nonceAndCounter;
    private readonly ICryptoTransform _counterEncryptor;
    private readonly Queue<byte> _xorMask = new Queue<byte>();
    private readonly SymmetricAlgorithm _symmetricAlgorithm;

    public CounterModeCryptoTransform(SymmetricAlgorithm symmetricAlgorithm, byte[] key, byte[] noncecounter)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        _symmetricAlgorithm = symmetricAlgorithm ?? throw new ArgumentNullException(nameof(symmetricAlgorithm));
        _nonceAndCounter = new byte[16];
        Array.Copy(noncecounter, 0,  _nonceAndCounter, 0, noncecounter.Length);

        var zeroIv = new byte[_symmetricAlgorithm.BlockSize / 8];
        _counterEncryptor = symmetricAlgorithm.CreateEncryptor(key, zeroIv);
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        var output = new byte[inputCount];
        TransformBlock(inputBuffer, inputOffset, inputCount, output, 0);
        return output;
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer,
        int outputOffset)
    {
        for (var i = 0; i < inputCount; i++)
        {
            if (NeedMoreXorMaskBytes())
            {
                EncryptCounterThenIncrement();
            }

            var mask = _xorMask.Dequeue();
            outputBuffer[outputOffset + i] = (byte) (inputBuffer[inputOffset + i] ^ mask);
        }

        return inputCount;
    }

    private bool NeedMoreXorMaskBytes()
    {
        return _xorMask.Count == 0;
    }

    private byte[] _counterModeBlock;
    private void EncryptCounterThenIncrement()
    {
        _counterModeBlock ??= new byte[_symmetricAlgorithm.BlockSize / 8];

        _counterEncryptor.TransformBlock(_nonceAndCounter, 0, _nonceAndCounter.Length, _counterModeBlock, 0);
        IncrementCounter();

        foreach (var b in _counterModeBlock)
        {
            _xorMask.Enqueue(b);
        }
    }

    private void IncrementCounter()
    {
        for(var i = 16; i > 0; i-- )
        {
            _nonceAndCounter[i - 1]++;
            if(_nonceAndCounter[i - 1] != 0 )
            {
                break;
            }
        }
    }

    public int InputBlockSize => _symmetricAlgorithm.BlockSize / 8;
    public int OutputBlockSize => _symmetricAlgorithm.BlockSize / 8;
    public bool CanTransformMultipleBlocks => true;
    public bool CanReuseTransform => false;

    public void Dispose()
    {
        _counterEncryptor.Dispose();
    }
}