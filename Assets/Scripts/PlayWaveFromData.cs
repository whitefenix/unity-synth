using UnityEngine;
using UnityEngine.Serialization;

//From https://discussions.unity.com/t/generating-a-simple-sinewave/665023/2
public class PlayWaveFromData : MonoBehaviour
{
    public float SampleRate = 44100;

    private float _phase;

    private AudioSource _audioSource;

    public Note[] Notes;

    private float _timeIndex;
    
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
            var time = i / (float)data.Length; //Does this work? Or do we need to account for duration?
            
            foreach (var note in Notes)
            {
                float angle = (_timeIndex * 2 * Mathf.PI * note.Frequency / SampleRate) + note.Phase;
                data[i] += Mathf.Sin(angle) * note.Ampltiude;
                data[i+1] += Mathf.Sin(angle) * note.Ampltiude;
            }

            _timeIndex++;

            if (_timeIndex >= SampleRate)
            {
                _timeIndex = 0;
            }
        }
    }
}
