//=============================
//Author: Zack Yang 
//Created Date: 10/29/2020 21:46
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class MyVideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public VideoClip a;
    private RawImage rawImage;
    [SerializeField]
    [Range(0f, 1f)] public float Fadespeed = 1f;

    private void Awake()
    {
        //获取Canvals对应的组件
        videoPlayer = this.GetComponent<VideoPlayer>();
        rawImage = this.GetComponent<RawImage>();
    }

    void Start()
    {
        //视频循环播放
        videoPlayer.isLooping = false;
        videoPlayer.clip = a;
    }


    void Update()
    {
        //如果videoPlayer没有对应的视频texture，则返回
        if (videoPlayer.texture == null)
        {
            return;
        }

        //把VideoPlayerd的视频渲染到UGUI的RawImage
        rawImage.texture = videoPlayer.texture;
        VideoFade();
    }
    //一个淡入的效果
    public void VideoFade()
    {
        videoPlayer.Play();
        rawImage.color = Color.Lerp(rawImage.color, Color.white, Fadespeed * Time.deltaTime);

    }


}

