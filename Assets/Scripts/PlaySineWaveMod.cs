using UnityEngine;

//From https://discussions.unity.com/t/generating-a-simple-sinewave/665023/2
public class PlaySineWaveMod : MonoBehaviour
{
    [Range(1, 20000)]
    public float Frequency;
    
    public float SampleRate = 44100;

    private float _phase;
    
    private AudioSource _audioSource;

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
        _audioSource.Play();
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            _phase += 2 * Mathf.PI * Frequency / SampleRate;

            data[i] = Mathf.Sin(_phase);

            if (channels == 2)
                data[i + 1] = Mathf.Sin(_phase);  

            if (_phase >= 2 * Mathf.PI)
            {
                _phase -= 2 * Mathf.PI;
            }
        }
    }
}
