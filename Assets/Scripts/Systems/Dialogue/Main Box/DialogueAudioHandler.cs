using UnityEngine;
using static SoundsAmount;

public class DialogueAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioHolder[] m_audios = new AudioHolder[VoicesAudiosAmount + SoundsAudiosAmount];
    private int m_voiceIndex;
    private int m_soundIndex;

    public void HandleVoice(AudioData voice)
    {
        m_voiceIndex = (m_voiceIndex + 1) % VoicesAudiosAmount;
        PlayAudio(voice, m_voiceIndex, 0);
    }
    
    public void HandleSound(AudioData sound)
    {
        m_soundIndex = (m_soundIndex + 1) % SoundsAudiosAmount;
        int actualSlot = m_soundIndex + VoicesAudiosAmount;
        PlayAudio(sound, actualSlot, 0);
    }

    public void PlayAudio(AudioData audio, int i, int j)
    {
        if (audio.Clip != null)
        {
            m_audios[i].AudioData[j] = audio;
            m_audios[i].ActivateSound(j);
        }
    }

    private void OnValidate()
    {
        bool areHoldersNull = false;
        foreach (AudioHolder holder in m_audios)
        {
            if (holder == null) areHoldersNull = true;
        }
        if (areHoldersNull)
        {
            m_audios = GetComponentsInChildren<AudioHolder>();
        }
        for (int i = 0; i < m_audios.Length; i++)
        {
            if (m_audios[i].AudioData.Length != 2)
            {
                m_audios[i].AudioData = new AudioData[2];
            }
        }
    }
}