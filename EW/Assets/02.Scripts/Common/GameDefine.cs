
public class GameDefine {

    // 개발인지 라이브인제 
    public static bool IsDevMode = true;

    // 로케일
    public static LocalType localType = LocalType.Ko;

    // 게임서능
    public const int GAME_FRAME = 30;
    public const float DEFAULT_TIMESCALE = 1f;
}

public enum LOGSTATE
{
    NORMAL = 0,
    WARRING,
    ERROR,
}

public enum PrevType
{
    Not,
    OnlyHide,
    Hide,
    OnlyClose,
    Close,
}

public enum LocalType
{
    Ko,
    En,
}