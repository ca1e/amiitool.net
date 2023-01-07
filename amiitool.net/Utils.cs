using System.Net;
using System.Runtime.CompilerServices;

namespace amiitool.net;

public static class Utils
{
    public static Random Rng = new Random();

    public static void RecreateDirectory(string dir)
    {
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }
        Directory.CreateDirectory(dir);
    }
    public static void CreateEmptyFile(string path)
    {
        File.Create(path).Dispose();
    }

    public static void PrepareNet()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    public static byte[] GetFromURL(string url)
    {
        PrepareNet();
        using var client = new HttpClient();
        return client.GetByteArrayAsync(url).Result;
    }

    public static void Unless(bool cond, string message)
    {
        if (!cond)
        {
            throw new Exception(message);
        }
    }

    public static ushort ReverseUInt16(ushort val)
    {
        var bytes = BitConverter.GetBytes(val);
        Array.Reverse(bytes);
        return BitConverter.ToUInt16(bytes, 0);
    }
}
