using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    private const string BgmVolumeParameter = "BGMVolume";
    private const string SfxVolumeParameter = "SFXVolume";

    [Header("Audio Mixer Groups")]
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    [Header("BGM")]
    public AudioClip[] bgmClips;
    [Range(0f, 1f)] public float bgmVolume;
    private AudioSource[] _bgmPlayers;
    private int _bgmChannelIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    [Range(0f, 1f)] public float sfxVolume;
    private AudioSource[] _sfxPlayers;
    private Dictionary<string, AudioSource> _activeSfx = new Dictionary<string, AudioSource>();
    private Dictionary<Sfx, AudioSource> _restrictedSfxInstances = new Dictionary<Sfx, AudioSource>();
    private Dictionary<(Transform, Sfx), AudioSource> _restrictedSfx = new();

    private GameObject _sfxObject;
    private GameObject _bgmObject;

    public enum Bgm
    {
        BGM1,
        BGM2,
        BGM3,
    }

    public enum Sfx
    {
        CarAcceleration,
        CarHorn,
        DigSoil,
        DroneFlying,
        DroneSpraySound,
        EngineStart,
        RunOnEarth,
        TomatoGrow,
        WalkOnEarth,
        WaterTomato,
        EnemyLaugh1,
        EnemyLaugh2,
        EnemyLaugh3,
        FenceBreak,
        EnemyFrustrated,
        PointUp,
        TomatoPick
    }

    public override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
        bgmVolume = Mathf.Clamp01(bgmVolume);
        sfxVolume = Mathf.Clamp01(sfxVolume);
        _bgmChannelIndex = bgmClips.Length;
        Init();
    }

    void Start()
    {
        RestoreAudioMixerSettings();
    }

    void Init()
    {
        _bgmObject = new GameObject("BGM");
        _bgmObject.transform.parent = transform;
        _bgmPlayers = new AudioSource[_bgmChannelIndex];
        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            _bgmPlayers[i] = _bgmObject.AddComponent<AudioSource>();
            _bgmPlayers[i].playOnAwake = false;
            _bgmPlayers[i].volume = bgmMixerGroup != null ? 1f : bgmVolume;
            _bgmPlayers[i].priority = 0;
            _bgmPlayers[i].loop = true;
            _bgmPlayers[i].outputAudioMixerGroup = bgmMixerGroup;
        }
        _sfxObject = new GameObject("SFXPlayer");
        _sfxObject.transform.parent = transform;
    }

    public void PlayBGM(Bgm bgm, bool isPlay)
    {
        int index = (int)bgm;
        if (index < 0 || index >= bgmClips.Length || bgmClips[index] == null)
        {
            // Debug.LogWarning($"BGM clip is not assigned: {bgm}", this);
            return;
        }

        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            if (isPlay && i != index)
            {
                _bgmPlayers[i].Stop();
                continue;
            }
            if (i != index) continue;

            var source = _bgmPlayers[i];
            if (!isPlay) source.Stop();
            else if (!source.isPlaying)
            {
                source.clip = bgmClips[index];
                source.Play();
            }
        }
    }

    public string PlaySfx(Sfx sfx, bool isLoop, float volume)
    {
        // 특정 사운드 중복 재생 방지
        if (sfx == Sfx.EnemyFrustrated || sfx == Sfx.FenceBreak)
        {
            if (_restrictedSfxInstances.ContainsKey(sfx) && _restrictedSfxInstances[sfx] != null && _restrictedSfxInstances[sfx].isPlaying)
            {
                // 이미 재생 중이면 중복 재생하지 않음
                return null;
            }
        }

        AudioSource source = _sfxObject.AddComponent<AudioSource>();
        source.clip = sfxClips[(int)sfx];
        source.volume = volume;
        source.outputAudioMixerGroup = sfxMixerGroup;
        source.dopplerLevel = 1.0f;
        source.reverbZoneMix = 0.0f;
        source.loop = isLoop;
        source.Play();

        string id = System.Guid.NewGuid().ToString();
        _activeSfx[id] = source;

        // 중복 방지용 딕셔너리에 등록
        if (sfx == Sfx.EnemyFrustrated || sfx == Sfx.FenceBreak)
        {
            _restrictedSfxInstances[sfx] = source;
            StartCoroutine(RemoveSfxWhenFinishedConditional(id, sfx, source));
        }
        else
        {
            StartCoroutine(RemoveSfxWhenFinished(id, source));
        }

        return id;
    }

    public string Play3DSfx(Sfx sfx, Transform followTarget, float volume = 1f)
    {
        bool preventDuplicate = (sfx == Sfx.EnemyFrustrated);
        var key = (followTarget, sfx);

        if (preventDuplicate)
        {
            if (_restrictedSfx.TryGetValue(key, out var existingSource))
            {
                if (existingSource != null && existingSource.isPlaying)
                {
                    return null;
                }
                else
                {
                    _restrictedSfx.Remove(key);
                }
            }
        }

        GameObject tempGo = new GameObject("TempAudio_" + sfx);
        tempGo.transform.position = followTarget.position;
        tempGo.transform.SetParent(followTarget);

        AudioSource source = tempGo.AddComponent<AudioSource>();
        source.clip = sfxClips[(int)sfx];
        source.volume = volume;
        source.outputAudioMixerGroup = sfxMixerGroup;
        source.spatialBlend = 1f;
        source.dopplerLevel = 1.0f;
        source.reverbZoneMix = 0.0f;
        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.minDistance = 10f;
        source.maxDistance = 30f;
        source.Play();

        string id = System.Guid.NewGuid().ToString();
        _activeSfx[id] = source;
        if (preventDuplicate)
            _restrictedSfx[key] = source;

        Destroy(tempGo, source.clip.length);
        StartCoroutine(RemoveSfxWhenFinishedConditional(id, preventDuplicate ? key : null, source));
        return id;
    }

    public string Play3DSfx(Sfx sfx)
    {
        AudioSource source = _sfxObject.AddComponent<AudioSource>();
        source.clip = sfxClips[(int)sfx];
        source.outputAudioMixerGroup = sfxMixerGroup;
        source.spatialBlend = 1f;
        source.dopplerLevel = 0.0f;
        source.reverbZoneMix = 0.0f;
        source.Play();

        string id = System.Guid.NewGuid().ToString();
        _activeSfx[id] = source;
        StartCoroutine(RemoveSfxWhenFinished(id, source));
        return id;
    }

    public void ChangeVolume(string id, float distance, float searchDistance)
    {
        if (_activeSfx.ContainsKey(id))
        {
            _activeSfx[id].volume = (sfxVolume - sfxVolume * (distance / searchDistance));
        }
    }

    public void StopSfx(string id)
    {
        if (id == null) return;
        if (_activeSfx.TryGetValue(id, out var source))
        {
            _activeSfx.Remove(id);
            if (source != null) source.Stop();
        }
    }

    public void StopAllSfx()
    {
        foreach (var source in _activeSfx.Values)
        {
            if (source == null) continue;
            source.Stop();
        }
        _activeSfx.Clear();
        RestoreAudioMixerSettings();
    }

    public void ChangeBgmVolume(float vol)
    {
        bgmVolume = Mathf.Clamp01(vol);
        for (int i = 0; i < _bgmPlayers.Length; i++)
        {
            _bgmPlayers[i].volume = bgmMixerGroup != null ? 1f : bgmVolume;
        }
        ApplyMixerVolume(bgmMixerGroup, BgmVolumeParameter, bgmVolume);
    }

    public void ChangeSfxVolume(float vol)
    {
        sfxVolume = Mathf.Clamp01(vol);
        if (sfxMixerGroup == null)
            foreach (var source in _activeSfx.Values)
                if (source != null) source.volume = sfxVolume;
        ApplyMixerVolume(sfxMixerGroup, SfxVolumeParameter, sfxVolume);
    }

    public void UIBgm(bool isPlay)
    {
        AudioHighPassFilter bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        bgmEffect.enabled = isPlay;
    }

    public void RestoreAudioMixerSettings()
    {
        ApplyMixerVolume(bgmMixerGroup, BgmVolumeParameter, bgmVolume);
        ApplyMixerVolume(sfxMixerGroup, SfxVolumeParameter, sfxVolume);
    }

    private static void ApplyMixerVolume(AudioMixerGroup group, string parameter, float volume)
    {
        if (group != null)
            group.audioMixer.SetFloat(parameter, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    private IEnumerator RemoveSfxWhenFinished(string id, AudioSource source)
    {
        if (source == null) yield break;
        yield return new WaitUntil(() => source == null || !source.isPlaying);

        if (_activeSfx.ContainsKey(id))
        {
            _activeSfx.Remove(id);
            Destroy(source);
        }
    }

    private IEnumerator RemoveSfxWhenFinishedConditional(string id, (Transform, Sfx)? key, AudioSource source)
    {
        if (source == null) yield break;
        yield return new WaitUntil(() => source == null || !source.isPlaying);

        if (_activeSfx.ContainsKey(id))
            _activeSfx.Remove(id);

        if (key.HasValue && _restrictedSfx.TryGetValue(key.Value, out var s) && s == source)
            _restrictedSfx.Remove(key.Value);

        if (source != null)
            Destroy(source);
    }
    
    private IEnumerator RemoveSfxWhenFinishedConditional(string id, Sfx sfxKey, AudioSource source)
    {
        if (source == null)
            yield break;

        yield return new WaitUntil(() => source == null || !source.isPlaying);

        if (_activeSfx.ContainsKey(id))
            _activeSfx.Remove(id);

        if (_restrictedSfxInstances.ContainsKey(sfxKey) && _restrictedSfxInstances[sfxKey] == source)
            _restrictedSfxInstances.Remove(sfxKey);

        if (source != null)
            Destroy(source);
    }
}
