using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public GameObject playObjectPrefab;
    public GameObject playObject;

    public SpriteRenderer illust_Background;
    public SpriteRenderer illust_Character;

    private void Awake()
    {
        playObject = Instantiate(playObjectPrefab);

        illust_Background = playObject.transform.Find("Illust_Background").GetComponent<SpriteRenderer>();
        illust_Character = playObject.transform.Find("Illust_Character").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ConversationManager.Instance.StartConversation();
    }

    public void UpdateBackground()
    {

    }

    public void UpdateChracter()
    {

    }
}
