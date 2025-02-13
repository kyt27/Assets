using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEngine : MonoBehaviour
{
    public static MusicEngine instance;

    public AudioClip[] clips;
    public float[] lengths;
    public float[] bpms;
    public string[] keys;
    private Dictionary<string, int> keyToIndex;
    private float channel1Timer;
    private float channel2Timer;
    public bool playChannel2;
    private bool c1aUse = true;
    private bool c2aUse = true;
    public GameObject channel1aObject;
    public GameObject channel1bObject;
    public GameObject channel2aObject;
    public GameObject channel2bObject;
    private AudioSource channel1a;
    private AudioSource channel2a;
    private AudioSource channel1b;
    private AudioSource channel2b;
    private AudioReverbFilter reverb1a;
    private AudioReverbFilter reverb1b;
    private AudioReverbFilter reverb2a;
    private AudioReverbFilter reverb2b;
    private AudioDistortionFilter distortion1a;
    private AudioDistortionFilter distortion1b;
    private AudioDistortionFilter distortion2a;
    private AudioDistortionFilter distortion2b;
    public bool channel1Loop;
    public bool channel2Loop;
    private int channel1Clip;
    private int channel2Clip;
    public float musicVolume;
    private bool fading1 = false;
    private bool fading2 = false;
    public float fade1Time;
    public float fade2Time;
    public float fade1Target;
    public float fade2Target;
    public bool stopAfterFade1 = false;
    public bool stopAfterFade2 = false;

    // Start is called before the first frame update
    void Start() {

    }

    public void Setup()
    {
        channel1a = channel1aObject.GetComponent<AudioSource>();
        channel1b = channel1bObject.GetComponent<AudioSource>();
        channel2a = channel2aObject.GetComponent<AudioSource>();
        channel2b = channel2bObject.GetComponent<AudioSource>();
        reverb1a = channel1aObject.GetComponent<AudioReverbFilter>();
        reverb1b = channel1bObject.GetComponent<AudioReverbFilter>();
        reverb2a = channel2aObject.GetComponent<AudioReverbFilter>();
        reverb2b = channel2bObject.GetComponent<AudioReverbFilter>();
        distortion1a = channel1aObject.GetComponent<AudioDistortionFilter>();
        distortion1b = channel1bObject.GetComponent<AudioDistortionFilter>();
        distortion2a = channel2aObject.GetComponent<AudioDistortionFilter>();
        distortion2b = channel2bObject.GetComponent<AudioDistortionFilter>();
        channel1a.volume = musicVolume;
        channel1b.volume = musicVolume;
        channel2a.volume = musicVolume;
        channel2b.volume = musicVolume;
        ControlReverb(3, true);
        ControlDistortion(3, false);
        keyToIndex = new Dictionary<string, int>();
        int index = 0;
        foreach (string i in keys)
        {
            keyToIndex[i] = index++;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Setup();
            instance = this;
        }
    }

    // Update is called once per frame
    void Update() {
        if (channel1Loop) {
            if (c1aUse) {
                if (channel1a.time > 60.0F * lengths[channel1Clip] / bpms[channel1Clip] - 1.0F) {
                    channel1b.PlayDelayed(60.0F * lengths[channel1Clip] / bpms[channel1Clip] - channel1a.time);
                    c1aUse = false;
                }
            } else {
                if (channel1b.time > 60.0F * lengths[channel1Clip] / bpms[channel1Clip] - 1.0F) {
                    channel1a.PlayDelayed(60.0F * lengths[channel1Clip] / bpms[channel1Clip] - channel1b.time);
                    c1aUse = true;
                }
            }
        }
        if (channel2Loop) {
            if (c2aUse) {
                if (channel2a.time > 60.0F * lengths[channel2Clip] / bpms[channel2Clip] - 1.0F) {
                    channel2b.PlayDelayed(60.0F * lengths[channel2Clip] / bpms[channel2Clip] - channel2a.time);
                    c2aUse = false;
                }
            } else {
                if (channel2b.time > 60.0F * lengths[channel2Clip] / bpms[channel2Clip] - 1.0F) {
                    channel2a.PlayDelayed(60.0F * lengths[channel2Clip] / bpms[channel2Clip] - channel2b.time);
                    c2aUse = true;
                }
            }
        }
        if (fading1) {
            float grad1 = (fade1Target - channel1a.volume) / fade1Time;
            channel1a.volume += grad1 * Time.deltaTime;
            channel1b.volume += grad1 * Time.deltaTime;
            fade1Time -= Time.deltaTime;
            if (fade1Time <= 0) {
                channel1a.volume = fade1Target;
                channel1b.volume = fade1Target;
                if (stopAfterFade1)
                {
                    channel1a.Stop();
                    channel1b.Stop();
                    stopAfterFade1 = false;
                }
                fading1 = false;
            }
        }
        if (fading2) {
            float grad2 = (fade2Target - channel2a.volume) / fade1Time;
            channel2a.volume += grad2 * Time.deltaTime;
            channel2b.volume += grad2 * Time.deltaTime;
            fade2Time -= Time.deltaTime;
            if (fade2Time <= 0)
            {
                channel2a.volume = fade2Target;
                channel2b.volume = fade2Target;
                if (stopAfterFade2)
                {
                    channel2a.Stop();
                    channel2b.Stop();
                    stopAfterFade2 = false;
                }
                fading2 = false;
            }
        }

        if (false)
        {
            if (Input.GetKeyDown("1"))
            {
                FadeToChannel1(1);
            }

            if (Input.GetKeyDown("2"))
            {
                FadeToChannel2(1);
            }
        }
    }

    public void Channel1Only() {
        channel1a.volume = musicVolume;
        channel1b.volume = musicVolume;
        channel2a.volume = 0;
        channel2b.volume = 0;
    }

    public void Channel2Only() {
        channel1a.volume = 0;
        channel1b.volume = 0;
        channel2a.volume = musicVolume;
        channel2b.volume = musicVolume;
    }

    public void FadeToChannel1(float fadeTime) {
        fade1Time = fadeTime;
        fade2Time = fadeTime;
        fade1Target = musicVolume;
        fade2Target = 0;
        fading1 = true;
        fading2 = true;
        stopAfterFade1 = false;
        stopAfterFade2 = false;
    }

    public void FadeToChannel2(float fadeTime) {
        fade1Time = fadeTime;
        fade2Time = fadeTime;
        fade1Target = 0;
        fade2Target = musicVolume;
        fading1 = true;
        fading2 = true;
        stopAfterFade1 = false;
        stopAfterFade2 = false;
    }

    public void FadeBothAndStop(float fadeTime) {
        fade1Time = fadeTime;
        fade2Time = fadeTime;
        fade1Target = 0;
        fade2Target = 0;
        fading1 = true;
        fading2 = true;
        stopAfterFade1 = true;
        stopAfterFade2 = true;
    }

    public void MuteChannel(int cases) {
        switch (cases)
        {
            case 1:
                channel1a.volume = 0;
                channel1b.volume = 0;
                break;
            case 2:
                channel2a.volume = 0;
                channel2b.volume = 0;
                break;
            case 3:
                channel1a.volume = 0;
                channel1b.volume = 0;
                channel2a.volume = 0;
                channel2b.volume = 0;
                break;
        }
    }

    public void StopChannel(int cases)
    {
        switch (cases)
        {
            case 1:
                channel1a.Stop();
                channel1b.Stop();
                break;
            case 2:
                channel2a.Stop();
                channel2b.Stop();
                break;
            case 3:
                channel1a.Stop();
                channel1b.Stop();
                channel2a.Stop();
                channel2b.Stop();
                break;
        }
    }

    public void ControlReverb(int cases, bool state)
    {
        switch (cases)
        {
            case 1:
                reverb1a.enabled = state;
                reverb1b.enabled = state;
                break;
            case 2:
                reverb2a.enabled = state;
                reverb2b.enabled = state;
                break;
            case 3:
                reverb1a.enabled = state;
                reverb1b.enabled = state;
                reverb2a.enabled = state;
                reverb2b.enabled = state;
                break;
        }
    }

    public void ControlDistortion(int cases, bool state)
    {
        switch (cases)
        {
            case 1:
                distortion1a.enabled = state;
                distortion1b.enabled = state;
                break;
            case 2:
                distortion2a.enabled = state;
                distortion2b.enabled = state;
                break;
            case 3:
                distortion1a.enabled = state;
                distortion1b.enabled = state;
                distortion2a.enabled = state;
                distortion2b.enabled = state;
                break;
        }
    }

    public void SetDistortion(int cases, float value)
    {
        switch (cases)
        {
            case 1:
                distortion1a.distortionLevel = value;
                distortion1b.distortionLevel = value;
                break;
            case 2:
                distortion2a.distortionLevel = value;
                distortion2b.distortionLevel = value;
                break;
            case 3:
                distortion1a.distortionLevel = value;
                distortion1b.distortionLevel = value;
                distortion2a.distortionLevel = value;
                distortion2b.distortionLevel = value;
                break;
        }
    }

    public void PlayOnChannel1(int clipNum, bool loop) {
        Debug.Log("Playing on Channel 1");
        channel1a.clip = clips[clipNum];
        channel1b.clip = clips[clipNum];
        channel1Clip = clipNum;
        channel1Loop = loop;
        c1aUse = true;
        channel1a.Stop();
        channel1b.Stop();
        channel1a.Play();
    }

    public void PlayOnChannel2(int clipNum, bool loop)
    {
        Debug.Log("Playing on Channel2");
        channel2a.clip = clips[clipNum];
        channel2b.clip = clips[clipNum];
        channel2Clip = clipNum;
        channel2Loop = loop;
        c2aUse = true;
        channel2a.Stop();
        channel2b.Stop();
        channel2a.Play();
    }

    public void PlayOnBoth(int clip1, int clip2, bool loop1, bool loop2)
    {
        Debug.Log("Playing on Channel Both");
        channel1a.clip = clips[clip1];
        channel1b.clip = clips[clip1];
        channel1Clip = clip1;
        channel1Loop = loop1;
        channel2a.clip = clips[clip2];
        channel2b.clip = clips[clip2];
        channel2Clip = clip2;
        channel2Loop = loop2;
        c1aUse = true;
        c2aUse = true;
        channel1a.Stop();
        channel2a.Stop();
        channel1b.Stop();
        channel2b.Stop();
        channel1a.Play();
        channel2a.Play();
    }

    public void PlayOnChannel1(string clipKey, bool loop)
    {
        PlayOnChannel1(keyToIndex[clipKey], loop);
    }

    public void PlayOnChannel2(string clipKey, bool loop)
    {
        PlayOnChannel2(keyToIndex[clipKey], loop);
    }

    public void PlayOnBoth(string clipKey1, string clipKey2, bool loop1, bool loop2)
    {
        PlayOnBoth(keyToIndex[clipKey1], keyToIndex[clipKey2], loop1, loop2);
    }
}