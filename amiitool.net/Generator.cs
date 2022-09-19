using System.Security.Cryptography;
using LibAmiibo.Data;
using LibAmiibo.Data.Figurine;

public static class Generator
{
    public static AmiiboTag Run(string id)
    {
        var bytes = new byte[540];
        var keygen = new byte[0x20];
        RandomNumberGenerator.Create().GetBytes(keygen);
        Array.Copy(keygen, 0, bytes, 0x1E8, keygen.Length);
        var amiiboData = AmiiboTag.FromInternalTag(new ArraySegment<byte>(bytes));
        amiiboData.Amiibo = Amiibo.FromStatueId(id);
        amiiboData.RandomizeUID();
        return amiiboData;
    }
}

