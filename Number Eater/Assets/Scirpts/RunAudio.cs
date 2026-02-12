using UnityEngine;

public class RunAudio : MonoBehaviour
{
    [SerializeField] AudioSource runWindSource;
    [SerializeField] AudioClip runWindClip;

    void Start()
    {
        runWindSource.clip = runWindClip;
        runWindSource.loop = true;
    }

    public void RunWindStart()
    {
        runWindSource.Play();
    }

    public void RunWindStop()
    {
        runWindSource.Stop();
    }
}
