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
    Pos = 1,
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

#region [ Setting ]

public enum SettingType
{ 
    Video = 1,
    Sound,
}

#endregion