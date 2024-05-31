using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharPosType characterPosType;

    SpriteRenderer spriteRenderer;
    RectTransform rectTransform;

    Sprite sprite;

    Vector3 currentPostion;
    Vector3 targetPosition;

    Vector3 currentScale;
    Vector3 targetScale;

    float duration = 1.0f;
    float elapsedTime = 0f;
    bool isTrans = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetCharacter_Sprite()
    {
        spriteRenderer.sprite = sprite;

        elapsedTime = 0f;
        Time.timeScale = 1f;  // Ensure time scale is normal
        Application.targetFrameRate = 60;
    }

    public void SetCharacter_Pos(CharPosType posType)
    {
        characterPosType = posType;

        switch(characterPosType)
        {
            case CharPosType.Left:
                rectTransform.anchoredPosition = PlayManager.Instance.character_LeftPos;
                break;
            case CharPosType.Center:
                rectTransform.anchoredPosition = PlayManager.Instance.character_CenterPos;
                break;
            case CharPosType.Right:
                rectTransform.anchoredPosition = PlayManager.Instance.character_RightPos;
                break;
            default:
                rectTransform.anchoredPosition = PlayManager.Instance.character_CenterPos;
                break;
        }
    }

    public void UpdateTrans(Vector3 pos, Vector3 scale)
    {
        if (isTrans)
            MoveEnd();

        isTrans = true;
        currentPostion = transform.localPosition;
        targetPosition = pos;
        currentScale = transform.localScale;
        targetScale = scale;
    }

    void MoveStart()
    {
        float t = elapsedTime / duration;
        transform.localPosition = Vector3.Lerp(currentPostion, targetPosition, t);
        transform.localScale = Vector3.Lerp(currentScale, targetScale, t);
    }

    void MoveEnd()
    {
        transform.localPosition = targetPosition;
        transform.localScale = targetScale;
        elapsedTime = 0f;
        isTrans = false;
    }

    private void Update()
    {
        if (isTrans)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
                MoveStart();
            else
                MoveEnd();
        }
    }
}
