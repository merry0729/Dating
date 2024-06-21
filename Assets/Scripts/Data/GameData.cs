using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    GameData_Conversation gameData_Conversation;
    // FriendShip
    public Dictionary<CharType, int> charFriendship = new Dictionary<CharType, int>();

    // Status
    public Dictionary<InfoStatusType, int> infoStatusDic = new Dictionary<InfoStatusType, int>();
    public Dictionary<CharStatusType, int> charStatusDic = new Dictionary<CharStatusType, int>();

    // Setting
    public ResolutionType resolutionType;
    public ScreenType screenType;
    public Dictionary<SoundType, float> soundVolumeDic = new Dictionary<SoundType, float>();
}

public class GameData_Conversation
{
    public int index;
}

public class GameData_Friendship
{

}

public class GameData_Status
{

}

public class GameData_Setting
{

}