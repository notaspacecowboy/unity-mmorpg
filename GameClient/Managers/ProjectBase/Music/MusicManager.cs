using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.LowLevel;

public class MusicManager : Singleton<MusicManager>
{
    public MusicManager()
    {
        MonoManager.Instance.AddUpdateListener(Update);
    }

    void Update()
    {
        for (int i = soundList.Count - 1; i >= 0; i--)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">背景音乐名</param>
    public void PlayBgm(string name)
    {
        //如果当前bgm为空,则在场景上新建一个用于防止audiosource组建的游戏对象,
        //并通过ResMgr从资源文件中加载名为name的音源文件,并开始播放
        if (bgm == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BGM player";
            bgm = obj.AddComponent<AudioSource>();
        }
        //异步加载背景音乐,加载完成后直接播放
        ResManager.Instance.LoadAsync<AudioClip>(ResManager.ResourceType.Music, name,
            (audioclip) =>
            {
                bgm.clip = audioclip;
                bgm.loop = true;
                bgm.Play();
            });
    }

    /// <summary>
    /// 改变背景音乐音量大小
    /// </summary>
    /// <param name="v">修改后的背景音乐音量大小值</param>
    public void ChangeBgmVolume(float v)
    {
        bgmVolume = v;
        if(bgm == null)
            return;
        bgm.volume = bgmVolume;
    }


    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBgm()
    {
        if (bgm == null)
            return;
        bgm.Pause();
    }

    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopBgm()
    {
        if (bgm == null)
            return;
        bgm.Stop();
    }

    /// <summary>
    ///播放一个音效
    /// </summary>
    /// <param name="name">音效名</param>
    /// <param name="isLoop">该音效是否循环播放</param>
    /// <param name="callBackAction">音效播放完成后的回调函数,默认为空</param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callBackAction = null)
    {
        if (soundPlayer == null)
        {
            soundPlayer = new GameObject();
            soundPlayer.name = "Sound Player";
        }

        //当音效资源异步加载结束后,再添加音效组件
        ResManager.Instance.LoadAsync<AudioClip>(ResManager.ResourceType.Sound, name, (audioClip) =>
        {
            AudioSource source = soundPlayer.AddComponent<AudioSource>();
            source.clip = audioClip;
            source.volume = bgmVolume;
            source.Play();
            soundList.Add(source);

            if (callBackAction != null)
                callBackAction(source);
        });
    }

    private void ChangeSoundVolume(float v)
    {
        soundVolume = v;
        foreach (var sound in soundList)
        {
            sound.volume = v;
        }
    }

    public void StopSound(AudioSource source)
    {
    }

    private AudioSource bgm = null;
    private float bgmVolume = 1;

    private GameObject soundPlayer = null;
    private List<AudioSource> soundList = new List<AudioSource>();
    private float soundVolume = 1;
}
