using System;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Serialization;

//From https://discussions.unity.com/t/generating-a-simple-sinewave/665023/2
public class PlaySineWave : MonoBehaviour
{
    [Range(1, 20000)]
    public float Frequency;

    [Range(1, 20000)]
    public float Frequency2;
    
    public float SampleRate = 44100;

    public float WaveLengthSeconds = 2.0f;

    private AudioSource _audioSource;

    private int _timeIndex = 0;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0; //make 2d
        _audioSource.volume = 0.05f;
        _audioSource.Stop();
    }

    [ContextMenu("Play")]
    public void Play()
    {
        _timeIndex = 0;
        _audioSource.Play();
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(_timeIndex, Frequency, SampleRate);

            if(channels == 2)
				data[i+1] = CreateSine(_timeIndex, Frequency2, SampleRate);
            
            _timeIndex++;

            if (_timeIndex >= (SampleRate * WaveLengthSeconds))
            {
                _timeIndex = 0;
            }
        }
    }

    private float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }
}
