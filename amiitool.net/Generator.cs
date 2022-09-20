using System.Security.Cryptography;
using LibAmiibo.Data;
using LibAmiibo.Data.Figurine;

namespace amiitool.net;

public static class Generator
{
    public static byte[] Create(string id, string nick = "ÔÆÇ³Ñ©")
    {
        var bytes = new byte[552];
        Array.Copy(Properties.Resources.tmp, bytes, 540);
        // Set Keygen Salt
        RandomNumberGenerator.Create().GetBytes(new Span<byte>(bytes, 0x1E8, 0x20));
        var amiiboData = AmiiboTag.FromInternalTag(new ArraySegment<byte>(bytes));
        // into the soul
        amiiboData.Amiibo = Amiibo.FromStatueId(id);
        amiiboData.AmiiboSettings.AmiiboUserData.AmiiboNickname = nick;
        amiiboData.RandomizeUID();
        return amiiboData.EncryptWithKeys();
    }
}

