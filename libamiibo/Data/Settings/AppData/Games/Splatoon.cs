using LibAmiibo.Attributes;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings.AppData.Games;

[AppID(0x10162B00)]
[AppDataInitializationTitleID("01003C703009C000")]
public class Splatoon2 : IGame
{
    private ArraySegment<byte> AppData { get; set; }

    public Splatoon2(ArraySegment<byte> appData)
    {
        this.AppData = appData;
    }

    [SupportedGame(typeof(Splatoon2))]
    public class Initializer : IAppDataInitializer
    {
        public void InitializeAppData(AmiiboTag tag)
        {
            this.ThrowOnInvalidAppId(tag);
            var game = new Splatoon2(tag.AppData);
            // TODO: Use for-loop and create extension method
        }
    }
}

[AppID(0x38600500)]
[AppDataInitializationTitleID("100C2503FC20000")]
public class Splatoon3 : IGame
{
    private ArraySegment<byte> AppData { get; set; }

    public Splatoon3(ArraySegment<byte> appData)
    {
        this.AppData = appData;
    }

    [SupportedGame(typeof(Splatoon2))]
    public class Initializer : IAppDataInitializer
    {
        public void InitializeAppData(AmiiboTag tag)
        {
            this.ThrowOnInvalidAppId(tag);
            var game = new Splatoon3(tag.AppData);
            // TODO: Use for-loop and create extension method
        }
    }
}