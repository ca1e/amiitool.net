using LibAmiibo.Data;
using amiitool.net;
using System.Text.Json;

#if DEBUG

#endif

if (args.Length == 0)
{
    Console.WriteLine(@"
Usage:
amiitool <mode> [<argument(s)>]

Mode:
-e|-d <input> <output>
-g [<amiiboID> <output>]

Exp:
amiitool -e rawdata.bin amiibodata.bin (encrypt generated raw data)
amiitool -g (generate all amiibo data to data/)");
    return;
}

string mode = args[0];

if(mode == "-g")
{
    if (args.Length == 3)
    {
        string input = args[1];
        string output = args[2];

        var data = Generator.Create(input);
        File.WriteAllBytes(output, data[..540]);
        return;
    }

    const string AmiiboAPIURL = "https://www.amiiboapi.com/api/amiibo/";
    var jd = Utils.GetFromURL(AmiiboAPIURL);
    using var jDoc = JsonDocument.Parse(jd);
    var count = jDoc.RootElement.GetProperty("amiibo").GetArrayLength();
    Console.WriteLine($"save {count} amiibos now...");
    Directory.CreateDirectory("data/");
    foreach (var entry in jDoc.RootElement.GetProperty("amiibo").EnumerateArray())
    {
        var aminame = entry.GetProperty("name").ToString();
        var amiid = entry.GetProperty("head").ToString() + entry.GetProperty("tail").ToString();
        System.Diagnostics.Debug.WriteLine(aminame);
        File.WriteAllBytes("data/" + amiid + ".bin", Generator.Create(amiid));
    }
    System.Diagnostics.Debug.WriteLine("debug done");
}
else
{
    if (args.Length != 3)
    {
        Console.WriteLine();
        return;
    }
    string input = args[1];
    string output = args[2];
    var inputData = File.ReadAllBytes(input);
    if (!File.Exists(input))
    {
        Console.WriteLine($"File not found:{input}!");
        return;
    }
    var flen = new FileInfo(input).Length;
    if (flen != 540 && flen != 532)
    {
        Console.WriteLine($"Invalid Amiibo data size. must be 540 bytes!");
        return;
    }
    switch (mode)
    {
        case "-e":
            var inputRaw = new byte[552];
            Array.Copy(inputData, inputRaw, inputData.Length);
            var amiiboData = AmiiboTag.FromInternalTag(new ArraySegment<byte>(inputRaw));

            var enc = amiiboData.EncryptWithKeys();
            File.WriteAllBytes(output, enc[..540]);
            break;
        case "-d":
            var amiiboTag = AmiiboTag.DecryptWithKeys(inputData);
            File.WriteAllBytes(output, amiiboTag?.InternalTag.Array?[..540] ?? new byte[540]);
            break;
        default:
            Console.WriteLine($"Unknown type: {mode}");
            break;
    }
}
