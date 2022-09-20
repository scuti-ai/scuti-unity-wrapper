using UnityEngine;

public enum ScutiLog
{
    None = 0,
    ErrorOnly = 1,
    Verbose = 2
}

[CreateAssetMenu(fileName = "ScutiSettings", menuName = "Scuti/ScutiSettings")]
public class ScutiSettings : ScriptableObject
{
    public string developerKey;
    public ScutiLog LogSettings = ScutiLog.ErrorOnly;
    //public enum CurrencyExchangeMethod
    //{
    //    None = 0,
    //    GameClient = 1,
    //    GameServer = 2
    //}
    //public enum AppOrientation
    //{
    //    None = 0,
    //    Autorotation = 1,
    //    Portrait = 2,
    //    Landscape
    //}
    //public CurrencyExchangeMethod ExchangeMethod = CurrencyExchangeMethod.GameClient;
    //public AppOrientation Orientation = AppOrientation.None;
    //public string secret;
    //
}