using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    ButtonClick,
    GetNumber,
    GameOverFallDown,
    GameOver,
    GameClearTurn,
    GameClearJump,
    Star1,
    Star2,
    Star3,
    GameClearText
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX Clips")]
    [SerializeField] private List<SFXClipData> sfxClips = new();

    private Dictionary<SFXType, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitDictionary();
    }

    private void InitDictionary()
    {
        sfxDict = new Dictionary<SFXType, AudioClip>();

        foreach (var data in sfxClips)
        {
            if (!sfxDict.ContainsKey(data.type))
                sfxDict.Add(data.type, data.clip);
        }
    }

    // 🔊 핵심 함수
    public void PlayShot(SFXType type, float volume = 1f)
    {
        if (!sfxDict.TryGetValue(type, out AudioClip clip))
        {
            Debug.LogWarning($"SFX not found: {type}");
            return;
        }

        sfxSource.PlayOneShot(clip, volume);
    }

    
}