using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] m_sprites;
    [SerializeField] private SpriteRenderer[] m_spriteRen;

    private void Start()
    {
        foreach (SpriteRenderer m_ren in m_spriteRen)
            m_ren.sprite = m_sprites[Random.Range(0, m_sprites.Length)];
    }
}
