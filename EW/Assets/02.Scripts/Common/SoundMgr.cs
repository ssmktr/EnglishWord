using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : Singleton<SoundMgr> {

    const string _path = "Sound/";

    public bool IsSfxSound = true;
    public bool IsBgSound = false;

    GameObject SfxBase, BgBase = null;

    AudioClip _Clip, _BgClip = null;
    AudioSource _SfxSource = null;
    AudioSource _BgSource = null;

    private void Awake()
    {
        if (gameObject.GetComponent<AudioListener>() == null)
            gameObject.AddComponent<AudioListener>();

        if (SfxBase == null)
        {
            SfxBase = new GameObject();
            SfxBase.name = "SFX";
            SfxBase.transform.parent = transform;
            if (SfxBase.GetComponent<AudioSource>() == null)
                SfxBase.AddComponent<AudioSource>();
            _SfxSource = SfxBase.GetComponent<AudioSource>();
            _SfxSource.playOnAwake = false;
            Debug.Log(_SfxSource);
        }

        if (BgBase == null)
        {
            BgBase = new GameObject();
            BgBase.name = "BG";
            BgBase.transform.parent = transform;
            if (BgBase.GetComponent<AudioSource>() == null)
                BgBase.AddComponent<AudioSource>();
            _BgSource = BgBase.GetComponent<AudioSource>();
            _BgSource.playOnAwake = false;
            Debug.Log(_BgSource);
        }
    }

    public void BgPlay(string soundName)
    {
        if (IsBgSound)
        {
            _BgClip = (AudioClip)Resources.Load(_path + soundName);
            _BgSource.clip = _BgClip;
            if (_BgSource.clip != null)
            {
                _BgSource.volume = 1f;
                _BgSource.loop = true;
                _BgSource.Play();
            }
        }
    }

    public void Play(string soundName)
    {
        if (IsSfxSound)
        {
            _Clip = (AudioClip)Resources.Load(_path + soundName);
            _SfxSource.clip = _Clip;
            if (_SfxSource.clip != null)
            {
                _SfxSource.volume = 1f;
                _SfxSource.loop = false;
                _SfxSource.Play();
            }
        }
    }
}
