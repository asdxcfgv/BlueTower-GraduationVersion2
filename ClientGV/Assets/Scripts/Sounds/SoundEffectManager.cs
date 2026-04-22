using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SoundEffectManager : SingletonMonobehaviour<SoundEffectManager>
{
    public int soundsVolume = 8;
    
    private Dictionary<SoundEffectSO, SoundEffect> loopingSoundDict = new Dictionary<SoundEffectSO, SoundEffect>();


    private void Start()
    {
        if (PlayerPrefs.HasKey("soundsVolume"))
        {
            soundsVolume = PlayerPrefs.GetInt("soundsVolume");
        }
       
        SetSoundsVolume(soundsVolume);
    }

    private void OnDisable()
    {
        // Save volume settings in playerprefs
        PlayerPrefs.SetInt("soundsVolume", soundsVolume);
    }

    /// <summary>
    /// Play the sound effect
    /// </summary>
    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        // Play sound using a sound gameobject and component from the object pool
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.soundPrefab, Vector3.zero, Quaternion.identity);
        sound.SetSound(soundEffect);
        sound.gameObject.SetActive(true);
        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length-0.2f));

    }
    
    /// <summary>
    /// 循环播放音效（可多次调用不会重复创建）
    /// </summary>
    public void PlayLoopingSound(SoundEffectSO soundEffect)
    {
        if (soundEffect == null || soundEffect.soundEffectClip == null) return;

        // 如果已经在播放，直接返回，不重复创建
        if (loopingSoundDict.ContainsKey(soundEffect))
        {
            return;
        }

        // 从对象池获取音效
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.soundPrefab, Vector3.zero, Quaternion.identity);
        sound.SetSound(soundEffect);
        
        // 开启循环
        sound.SetLoop(true);
        
        sound.gameObject.SetActive(true);

        // 加入字典管理
        loopingSoundDict.Add(soundEffect, sound);
    }

    /// <summary>
    /// 停止指定的循环音效
    /// </summary>
    public void StopLoopingSound(SoundEffectSO soundEffect)
    {
        if (soundEffect == null || !loopingSoundDict.ContainsKey(soundEffect)) return;

        SoundEffect sound = loopingSoundDict[soundEffect];
        if (sound != null)
        {
            sound.SetStop();
            sound.gameObject.SetActive(false);
        }

        loopingSoundDict.Remove(soundEffect);
    }

    /// <summary>
    /// 停止所有循环音效
    /// </summary>
    public void StopAllLoopingSounds()
    {
        foreach (var sound in loopingSoundDict.Values)
        {
            if (sound != null)
            {
                sound.SetStop();
                sound.gameObject.SetActive(false);
            }
        }

        loopingSoundDict.Clear();
    }


    /// <summary>
    /// Disable sound effect object after it has played thus returning it to the object pool
    /// </summary>
    private IEnumerator DisableSound(SoundEffect sound, float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration);
        sound.gameObject.SetActive(false);
    }

    //
    /// <summary>
    /// Increase sounds volume
    /// </summary>
    public void IncreaseSoundsVolume()
    {
        int maxSoundsVolume = 20;

        if (soundsVolume >= maxSoundsVolume) return;

        soundsVolume += 1;

        SetSoundsVolume(soundsVolume); ;
    }

    /// <summary>
    /// Decrease sounds volume
    /// </summary>
    public void DecreaseSoundsVolume()
    {
        if (soundsVolume == 0) return;

        soundsVolume -= 1;

        SetSoundsVolume(soundsVolume);
    }

    /// <summary>
    /// Set sounds volume
    /// </summary>
    private void SetSoundsVolume(int soundsVolume)
    {
        float muteDecibels = -80f;

        if (soundsVolume == 0)
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", Global.LinearToDecibels(soundsVolume));
        }
    }
}
