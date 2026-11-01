using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBasics : MonoBehaviour
{
    protected float m_audioVolume;
    public virtual float AudioVolume
    {
        get => m_audioVolume;
        protected set
        {
            m_audioVolume = value;
            m_audioVolume = Mathf.Clamp01(m_audioVolume);
        }
    }

    public void SetAudio(float value)
    {
        AudioVolume = value;
    }
}