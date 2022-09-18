using LibAmiibo.Data;
var data = File.ReadAllBytes("D:\\goblin.bin");
AmiiboTag.DecryptWithKeys(data);

if (args.Length != 3)
{
    Console.WriteLine($"Usage: amiitool (-e|-d) <input> <output>");
    return;
}

string mode = args[0];
string input = args[1];
string output = args[2];

if (new FileInfo(input).Length != 540)
{
    Console.WriteLine($"Invalid Amiibo data size. must be 540 bytes!");
    return;
}
var inputData = File.ReadAllBytes(input);

switch(mode)
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
        File.WriteAllBytes(output, amiiboTag.InternalTag.Array[..540]);
        break;
    default:
        Console.WriteLine($"Unknown type: {mode}");
        break;
}
