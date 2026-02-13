using UnityEngine;

public class PerformanceSettings : MonoBehaviour
{
    [SerializeField] private int targetFps = 60; // 30도 고려 가능

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // VSync 끄고 직접 fps 제한 (모바일 권장)
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;
    }
}