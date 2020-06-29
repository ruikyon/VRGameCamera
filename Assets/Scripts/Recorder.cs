using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using System.IO;

public class Recorder: MonoBehaviour
{
    private MediaEncoder mediaEncoder;
    private NativeArray<float> audioBuffer;
    [SerializeField] private RenderTexture recordTexture;

    private void Update()
    {
    }

    public void RecordMovie()
    {
        var videoAttr = new VideoTrackAttributes
        {
            frameRate = new MediaRational(50),
            width = 320,
            height = 200,
            includeAlpha = false
        };

        var audioAttr = new AudioTrackAttributes
        {
            sampleRate = new MediaRational(48000),
            channelCount = 2,
            language = "jp"
        };

        int sampleFramesPerVideoFrame = audioAttr.channelCount *
            audioAttr.sampleRate.numerator / videoAttr.frameRate.numerator;

        var encodedFilePath = Path.Combine(Path.GetTempPath(), "my_movie.mp4");

        Texture2D tex = new Texture2D((int)videoAttr.width, (int)videoAttr.height, TextureFormat.RGBA32, false);

        mediaEncoder = new MediaEncoder(encodedFilePath, videoAttr, audioAttr);
        audioBuffer = new NativeArray<float>(sampleFramesPerVideoFrame, Allocator.Temp);
    }

    public async void AddData() 
    {
        var request = AsyncGPUReadback.Request(recordTexture);
        while (!request.done) 
        {
            
        }
        //mediaEncoder.AddFrame();
        //mediaEncoder.AddSamples(audioBuffer);
    }
}