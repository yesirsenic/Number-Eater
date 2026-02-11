using UnityEngine;
using System.Collections;

public class UserClear : MonoBehaviour
{
    public void ClearAnimator()
    {
        if(GameManager.Instance.is_Clear)
        {
            StartCoroutine(GameClear());
        }

        else
        {
            StartCoroutine(GameOver());
        }
    }


    IEnumerator GameClear()
    {
        float totalDuration = 100f / 60f; // 1.6666667s
        float elapsed = 0f;

        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (elapsed < totalDuration)
        {
            elapsed += Time.deltaTime;

            // =========================
            // Y 위치 계산
            // =========================
            float yOffset = 0f;

            // 0 ~ 60프레임 (0 → 1 → 0)
            if (elapsed <= 1f)
            {
                float t = elapsed / 1f; // 0~1
                yOffset = 1f - Mathf.Abs(1f - t * 2f);
            }
            // 60 ~ 100프레임 (다시 0 → 1 → 0)
            else
            {
                float t = (elapsed - 1f) / (40f / 60f); // 0~1
                yOffset = 1f - Mathf.Abs(1f - t * 2f);
            }

            transform.localPosition = startPos + Vector3.up * yOffset;

            // =========================
            // Y 회전 (0 ~ 60프레임만)
            // =========================
            if (elapsed <= 1f)
            {
                float rotT = elapsed / 1f;
                float yAngle = 360f * rotT;
                transform.localRotation = startRot * Quaternion.Euler(0f, yAngle, 0f);
            }

            yield return null;
        }

        // 🔒 보정
        transform.localPosition = startPos;
        transform.localRotation = startRot;

        yield return new WaitForSeconds(0.25f);

        GameManager.Instance.GameEndPopupOn();
    }

    IEnumerator GameOver()
    {
        float duration = 1f;          // 60프레임 = 1초
        float half = duration / 2f;   // 30프레임 = 0.5초
        float elapsed = 0f;

        Quaternion startRot = transform.localRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float xAngle;

            if (elapsed <= half)
            {
                // 0 ~ 30프레임 : 0 → -30
                float t = elapsed / half;
                xAngle = Mathf.Lerp(0f, -30f, t);
            }
            else
            {
                // 30 ~ 60프레임 : -30 → -85
                float t = (elapsed - half) / half;
                xAngle = Mathf.Lerp(-30f, -85f, t);
            }

            transform.localRotation = startRot * Quaternion.Euler(xAngle, 0f, 0f);

            yield return null;
        }

       
        // 🔒 오차 보정
        transform.localRotation = startRot * Quaternion.Euler(-85f, 0f, 0f);

        yield return new WaitForSeconds(0.25f);

        GameManager.Instance.GameEndPopupOn();
    }


}
