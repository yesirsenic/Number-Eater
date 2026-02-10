using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NumberRunnerController : MonoBehaviour
{
    [Header("Number")]
    public float distancePerNumber = 1f;

    [Header("Run Timing")]
    [SerializeField] private float baseRunDuration = 2.5f;
    [SerializeField] private float maxRunDuration = 4f;

    [Header("Run Curve")]
    [SerializeField] private AnimationCurve speedCurve;
    // 0 → 1 → 0 형태 (가속 → 최고속 → 감속)

    [Header("Character Visual")]
    [SerializeField] private Transform characterVisual;
    [SerializeField] private float minLeanAngle = 12f;
    [SerializeField] private float maxLeanAngle = 30f;

    private Coroutine runCoroutine;
    private bool isRunning;

    // =============================
    // 외부에서 호출하는 진입 함수
    // =============================
    public void RunStart()
    {
        int currentNumber = GameManager.Instance.getNumberSum;
        float distance = currentNumber * distancePerNumber + 50f;

        if (runCoroutine != null)
            StopCoroutine(runCoroutine);


        GameManager.Instance.SetClear();
        runCoroutine = StartCoroutine(RunForward(distance, currentNumber));
    }

    // =============================
    // 핵심 이동 + 연출 코루틴
    // =============================

    IEnumerator RunForward(float distance, int number)
    {
        Vector3 startPos = transform.position;
        Vector3 dir = Vector3.forward;
        float moved = 0f;

        float duration =1.351f * Mathf.Log(number + 1f) - 2.327f;
        duration = Mathf.Clamp(duration, 0.1f, maxRunDuration);

        float leanAngle = Mathf.Clamp(number * 0.8f, minLeanAngle, maxLeanAngle);

        // ✅ speedCurve 평균(적분값) 구해서 정규화 (이게 핵심)
        const int SAMPLE = 120;
        float area = 0f;
        for (int i = 0; i < SAMPLE; i++)
        {
            float t0 = (float)i / SAMPLE;
            float t1 = (float)(i + 1) / SAMPLE;
            float y0 = Mathf.Max(0f, speedCurve.Evaluate(t0));
            float y1 = Mathf.Max(0f, speedCurve.Evaluate(t1));

            // ✅ dt(1/SAMPLE) 포함해서 "진짜 면적"으로
            area += (y0 + y1) * 0.5f * (1f / SAMPLE);
        }
        if (area <= 0.0001f) area = 1f;

        // 평균 속도 = distance / duration
        float baseSpeed = distance / duration;

        float elapsed = 0f;
        while (elapsed < duration && moved < distance)
        {
            float t = elapsed / duration;

            // ✅ 가속/감속용 속도(0→1→0)
            float speed01 = Mathf.Max(0f, speedCurve.Evaluate(t));

            // ✅ 정규화: speed01의 평균이 1이 되도록 보정
            float speed = baseSpeed * (speed01 / area);

            float step = speed * Time.deltaTime;
            moved = Mathf.Min(distance, moved + step);

            transform.position = startPos + dir * moved;

            // 숙였다가 복귀(앞으로 숙이는 각도)
            float lean = Mathf.Sin(t * Mathf.PI) * leanAngle;
            characterVisual.localRotation = Quaternion.Euler(-lean, 180f, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // ✅ 여기선 거의 도착해있을 거라 스냅이 안 느껴짐
        transform.position = startPos + dir * distance;
        characterVisual.localRotation = Quaternion.Euler(0f, 180f, 0f);

        GameManager.Instance.StartGameEndAniamtor();
    }

}
