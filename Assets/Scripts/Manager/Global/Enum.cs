#region [ SceneControlManager ]

// SceneControlManager.
public enum SceneType
{
    IntroScene,
    PlayScene,
}

#endregion

#region [ UIManager]

// UIManager.
public enum WindowUIType
{
    SettingUI,
}

#endregion

#region [ ObjectPoolManager ]

// ObjectPoolManager.
public enum PoolType
{
    Conversation,
    Message_Sender,
    Message_Mine,
}

public enum PoolParentType
{
    UI,
    GameObject,
}

#endregion

#region [ Conversation ]

public enum ConSetType
{
    Pos = 1,
}

public enum ConPosType
{
    Top,
    Middle,
    Bottom,
}

#endregion

#region [ 2D Object ]

public enum CharType
{
    Main,
    Heroin_1,
    Heroin_2,
    Heroin_3,
}

public enum CharDataType
{
    Pos,
    Scale,
}

public enum CharPosType
{
    Left,
    Center,
    Right,
}

public enum CharScaleType
{
    Down,
    Normal,
    Up,
}

#endregion

#region [ Phone ]

public enum PhoneState
{
    Messenger,
    Application,
}

public enum ApplicationType
{
    Messenger,
}

#endregion

#region [ Options ]

public enum OptionType
{ 
    Positive,
    Negative,
    Neutral,
    Non_Response,
}

#endregion

#region [ Setting ]

public enum SettingType
{ 
    Video,
    Sound,
}

public enum DropDownType
{ 
    Resolution,
    ScreenMode,
}

public enum ResolutionType
{
    W800_H600,
    W1920_H1080,
    W2560_H1440,
}

public enum ScreenType
{
    FullScreen,
    WindowScreen,
}

#endregion

#region [ Sound ]

public enum SoundType
{
    Master,
    BGM,
    Voice,
    SFX,
}

#endregion

