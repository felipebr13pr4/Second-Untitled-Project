using UnityEngine;

public class AudioHolder : AudioPlayer
{
    [SerializeField] private AudioData[] m_audioData = new AudioData[4];
    public AudioData[] AudioData { get => m_audioData; set => m_audioData = value; }
    public override float AudioVolume { get => AudioController.Instance.AudioVolume; }

    public void ActivateSound(params int[] indices)
    {
        for (int i = 0; i < indices.Length; i++)
            PlayAudio(m_audioData[indices[i]]);
    }

    public void ActivateStoppableSound(params int[] indices)
    {
        for (int i = 0; i < indices.Length; i++)
            PlayStoppableAudio(m_audioData[indices[i]]);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        for (int i = 0; i < m_audioData.Length; i++)
        {
            if (m_audioData[i].Clip == null) continue;
            float pitch;
            float min;
            float max;
            AudioData defaultData = new("None");
            if (!m_audioData[i].IsPitchRandom)
            {
                pitch = m_audioData[i].Pitch;
                if (m_audioData[i].Pitch == 0) pitch = defaultData.Pitch;
                m_audioData[i] = new(m_audioData[i].Clip, pitch,
                    m_audioData[i].IsPitchRandom, m_audioData[i].Min, m_audioData[i].Max);
            }
            else
            {
                min = m_audioData[i].Min;
                max = m_audioData[i].Max;
                if (m_audioData[i].Min == 0) min = defaultData.Min;
                if (m_audioData[i].Max == 0) max = defaultData.Max;
                m_audioData[i] = new(m_audioData[i].Clip, m_audioData[i].Pitch,
                    m_audioData[i].IsPitchRandom, min, max);
            }
        }
    }
}