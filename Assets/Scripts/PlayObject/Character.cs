using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharPosType characterPosType;

    SpriteRenderer spriteRenderer;
    RectTransform rectTransform;

    Sprite sprite;

    Vector3 targetPosition;
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
        targetPosition = pos;
        targetScale = scale;
        isTrans = true;
    }

    private void Update()
    {
        if (isTrans)
        {
            elapsedTime += Time.deltaTime;

            // 경과 시간이 duration을 초과하지 않은 경우 위치를 업데이트
            if (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, t);
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
            }
            else
            {
                // 이동이 완료된 경우 정확히 목표 위치로 설정하고 이동 종료
                transform.localPosition = targetPosition;
                transform.localScale = targetScale;
                isTrans = false;
            }
        }
    }
}
