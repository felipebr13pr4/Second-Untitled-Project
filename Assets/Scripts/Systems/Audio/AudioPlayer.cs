using UnityEngine;

public class AudioPlayer : AudioBasics
{
    public override float AudioVolume
    {
        get => base.AudioVolume;
        protected set
        {
            base.AudioVolume = value;
            m_audioSource.volume = m_audioVolume;
            m_stoppableAudioSource.volume = m_audioVolume;

        }
    }

    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] private AudioSource m_stoppableAudioSource;
    private bool m_generatedAudios;

    protected virtual void OnValidate()
    {
        if (!m_generatedAudios)
        {
            int i = gameObject.GetComponents<AudioSource>().Length;
            if (i < 2)
            {
                if (m_audioSource == null)
                    if (i == 1)
                        m_audioSource = gameObject.GetComponent<AudioSource>();
                    else
                        m_audioSource = gameObject.AddComponent<AudioSource>();
                if (m_stoppableAudioSource == null)
                    m_stoppableAudioSource = gameObject.AddComponent<AudioSource>();
            }
            m_generatedAudios = true;
        }
    }

    public void PlayAudio(AudioData data)
    {
        float pitch = data.Pitch;
        if (data.IsPitchRandom) pitch = Random.Range(data.Min, data.Max);
        m_audioSource.pitch = pitch;
        m_audioSource.PlayOneShot(data.Clip, AudioVolume);
    }

    public void PlayStoppableAudio(AudioData data)
    {
        float pitch = data.Pitch;
        if (data.IsPitchRandom) pitch = Random.Range(data.Min, data.Max);
        m_stoppableAudioSource.pitch = pitch;
        m_stoppableAudioSource.Stop();
        m_stoppableAudioSource.PlayOneShot(data.Clip, AudioVolume);
    }
}