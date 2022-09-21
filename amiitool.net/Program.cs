using LibAmiibo.Data;
using amiitool.net;

#if DEBUG
using LibAmiibo.Data.Settings.UserData.Mii;
Generator.Create("0000010000190002");
var ami = new byte[0x60];
var iii = File.ReadAllBytes(@"D:\github\amiitool.net\mii\Supper Mario Broth.charinfo");
Array.Copy(iii, 0, ami, 0x8, iii.Length);
var mii = new AmiiboMii(new ArraySegment<byte>(ami));
mii.CheckCRC();
#endif
if (args.Length != 3)
{
    Console.WriteLine($"Usage: amiitool (-e|-d) <input> <output>");
    return;
}

string mode = args[0];
string input = args[1];
string output = args[2];

if(mode != "-g")
{
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
}

var inputData = File.Exists(input) ? File.ReadAllBytes(input) : null;

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
    case "-g":
        var data = Generator.Create(input);
        File.WriteAllBytes(output, data[..540]);
        break;
    default:
        Console.WriteLine($"Unknown type: {mode}");
        break;
}
