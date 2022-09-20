using System.Security.Cryptography;
using LibAmiibo.Data;
using LibAmiibo.Data.Figurine;

public static class Generator
{
    public static byte[] Create(string id, string nick = "ca1e", string miiNick = "云浅雪")
    {
        var bytes = new byte[552];
        // Set BCC, Internal, Static Lock, and CC
        Array.Copy(new byte[] { 0x65, 0x48, 0x0F, 0xE0, 0xF1, 0x10, 0xFF, 0xEE }, bytes, 8);
        bytes[0x28] = 0xA5;
        // Set Dynamic Lock, and RFUI
        Array.Copy(new byte[] { 0x01, 0x00, 0x0F, 0xBD }, 0, bytes, 0x208, 4);
        // Set CFG0
        Array.Copy(new byte[] { 0x00, 0x00, 0x00, 0x04 }, 0, bytes, 0x20C, 4);
        // Set CFG1
        Array.Copy(new byte[] { 0x5F, 0x00, 0x00, 0x00 }, 0, bytes, 0x210, 4);
        // Set Keygen Salt
        RandomNumberGenerator.Create().GetBytes(new Span<byte>(bytes, 0x1E8, 0x20));
        var amiiboData = AmiiboTag.FromInternalTag(new ArraySegment<byte>(bytes));
        // into the soul
        amiiboData.Amiibo = Amiibo.FromStatueId(id);
        amiiboData.AmiiboSettings.AmiiboUserData.AmiiboNickname = nick;
        amiiboData.AmiiboSettings.AmiiboUserData.OwnerMii.MiiNickname = miiNick;
        amiiboData.RandomizeUID();
        return amiiboData.EncryptWithKeys();
    }
}

