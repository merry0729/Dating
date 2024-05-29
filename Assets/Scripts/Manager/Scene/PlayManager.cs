using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayManager : Singleton<PlayManager>
{
    public GameObject playObjectPrefab;
    public GameObject playObject;

    public Background illust_Background;
    public Character illust_Character_Main;

    public CharacterSettingTable characterSettingTable;
    public CharacterSettingData characterSettingData;

    List<Vector3> charPosList = new List<Vector3>();
    List<Vector3> charScaleList = new List<Vector3>();

    public Vector3 character_LeftPos;
    public Vector3 character_CenterPos;
    public Vector3 character_RightPos;

    public Vector3 character_DownScale;
    public Vector3 character_NormalScale;
    public Vector3 character_UpScale;

    private void Awake()
    {
        playObject = Instantiate(playObjectPrefab);

        illust_Background = playObject.transform.Find("Illust_Background").GetComponent<Background>();
        illust_Character_Main = playObject.transform.Find("Illust_Character_Main").GetComponent<Character>();

        SetCharacterSettingData();
    }

    private void Start()
    {
        
    }

    public void PlayStart()
    {
        ConversationManager.Instance.StartConversation();
    }

    public void SetBackground()
    {

    }

    public void SetChracter(int charIndex, int posIndex, int scaleIndex)
    {
        switch(charIndex)
        {
            case (int)CharType.Main:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
            case (int)CharType.Heroin_1:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
            case (int)CharType.Heroin_2:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
            case (int)CharType.Heroin_3:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
        }
    }


    void SetCharacterSettingData()
    {
        // CharacterSetting Table.
        characterSettingTable = CharacterSettingData.Table;
        // CharacterSetting Pos.
        characterSettingData = characterSettingTable.TryGet((int)CharDataType.Pos);

        // CharacterSetting Pos Data.
        charPosList.Add(new Vector3(characterSettingData.Vec[(int)CharPosType.Left][0],
                                    characterSettingData.Vec[(int)CharPosType.Left][1],
                                    characterSettingData.Vec[(int)CharPosType.Left][2]));

        charPosList.Add(new Vector3(characterSettingData.Vec[(int)CharPosType.Center][0],
                                    characterSettingData.Vec[(int)CharPosType.Center][1],
                                    characterSettingData.Vec[(int)CharPosType.Center][2]));

        charPosList.Add(new Vector3(characterSettingData.Vec[(int)CharPosType.Right][0],
                                    characterSettingData.Vec[(int)CharPosType.Right][1],
                                    characterSettingData.Vec[(int)CharPosType.Right][2]));


        // CharacterSetting Scale.
        characterSettingData = characterSettingTable.TryGet((int)CharDataType.Scale);

        // CharacterSetting Scale Data.
        charScaleList.Add(new Vector3(characterSettingData.Vec[(int)CharScaleType.Down][0],
                                      characterSettingData.Vec[(int)CharScaleType.Down][1],
                                      characterSettingData.Vec[(int)CharScaleType.Down][2]));

        charScaleList.Add(new Vector3(characterSettingData.Vec[(int)CharScaleType.Normal][0],
                                      characterSettingData.Vec[(int)CharScaleType.Normal][1],
                                      characterSettingData.Vec[(int)CharScaleType.Normal][2]));

        charScaleList.Add(new Vector3(characterSettingData.Vec[(int)CharScaleType.Up][0],
                                      characterSettingData.Vec[(int)CharScaleType.Up][1],
                                      characterSettingData.Vec[(int)CharScaleType.Up][2]));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            PlayStart();
    }
}
