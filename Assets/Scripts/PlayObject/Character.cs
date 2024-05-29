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

            // ��� �ð��� duration�� �ʰ����� ���� ��� ��ġ�� ������Ʈ
            if (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, t);
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
            }
            else
            {
                // �̵��� �Ϸ�� ��� ��Ȯ�� ��ǥ ��ġ�� �����ϰ� �̵� ����
                transform.localPosition = targetPosition;
                transform.localScale = targetScale;
                isTrans = false;
            }
        }
    }
}
