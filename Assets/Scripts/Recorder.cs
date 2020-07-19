using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Media;
using Unity.Collections;

//録画機能については一旦保留
public class Recorder: MonoBehaviour
{
    private MediaEncoder mediaEncoder;
    private bool isRecording;
    private float[] audioData;
    private Queue<Texture2D> textures;

    [SerializeField] private RenderTexture recordTexture;

    private void Awake()
    {
        isRecording = false;
        textures = new Queue<Texture2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (isRecording) 
            {
                EndRecord();
            }
            else
            {
                StartRecord();
            }
        }

        if(textures.Count > 0)
        {
            mediaEncoder.AddFrame(textures.Dequeue());
        }
    }

    public void StartRecord()
    {
        var videoAttr = new VideoTrackAttributes
        {
            frameRate = new MediaRational(30),
            width = (uint) recordTexture.width,
            height = (uint) recordTexture.height,
            includeAlpha = false
        };

        var audioAttr = new AudioTrackAttributes
        {
            sampleRate = new MediaRational(48000),
            channelCount = 2,
            language = "jp"
        };

        var time = DateTime.Now;

        var encodedFilePath = Path.Combine(Path.GetTempPath(), time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + ".mp4");
        Debug.Log(encodedFilePath);

        mediaEncoder = new MediaEncoder(encodedFilePath, videoAttr, audioAttr);
        isRecording = true;
        StartCoroutine(Record());
    }

    public void EndRecord() 
    {
        isRecording = false;
        StartCoroutine(StopRecord());
    }

    private IEnumerator StopRecord()
    {
        while (textures.Count > 0)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Fin record");

        mediaEncoder.Dispose();

    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (!isRecording) return;
        audioData = data;
    }

    private IEnumerator Record() 
    {
        while (audioData == null)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("check");
        while(isRecording)
        {
            AsyncGPUReadback.Request(recordTexture, 0, OnRequestComplete);
            //var audioBuffer = new NativeArray<float>(audioData, Allocator.Temp);
            //mediaEncoder.AddSamples(audioBuffer);
            //audioBuffer.Dispose();

            yield return new WaitForSecondsRealtime(1 / 30);
        }
    }

    private void OnRequestComplete(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
        }
        else
        {
            Debug.Log("no error");
            var buffer = request.GetData<Color32>();
            var tempTexture = new Texture2D(recordTexture.width, recordTexture.height, TextureFormat.RGBA32, false);
            tempTexture.LoadRawTextureData(buffer);
            tempTexture.Apply();
            textures.Enqueue(tempTexture);
        }
    }
}