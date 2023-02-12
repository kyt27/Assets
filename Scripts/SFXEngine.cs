using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXEngine : MonoBehaviour
{
    public AudioClip[] clips;
    public string[] keys;
    private Dictionary<string, int> keyToIndex;
    public AudioSource[] channels;
    public float sfxVolume = 1;

    // Start is called before the first frame update
    void Start()
    {
        keyToIndex = new Dictionary<string, int>();
        int index = 0;
        foreach (string i in keys)
        {
            keyToIndex[i] = index++;
        }
        foreach (AudioSource i in channels) {
            i.volume = sfxVolume;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("3"))
        {
            PlayClip("Blip");
        }
        if (Input.GetKeyDown("4"))
        {
            PlayClip("Footstep");
        }
        if (Input.GetKeyDown("5"))
        {
            PlayClip("Glitchy_Wind");
        }
        if (Input.GetKeyDown("6"))
        {
            PlayClip("Hurt");
        }
        if (Input.GetKeyDown("7"))
        {
            PlayClip("Quick_Test_Jump");
        }
        if (Input.GetKeyDown("8"))
        {
            PlayClip("Shoot");
        }
        if (Input.GetKeyDown("9"))
        {
            PlayClip("Slash");
        }
    }

    void PlayClip (int clipNum)
    {
        foreach (AudioSource i in channels) {
            if (!i.isPlaying) {
                i.clip = clips[clipNum];
                i.Play();
                break;
            }
        }
    }

    void PlayClip(string clipKey)
    {
        PlayClip(keyToIndex[clipKey]);
    }
}
