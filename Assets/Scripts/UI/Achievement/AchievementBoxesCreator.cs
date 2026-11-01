using System;
using UnityEngine;

public class AchievementBoxesCreator : MonoBehaviour
{
    [SerializeField] private BoxesToSpawn[] m_boxes;
    [SerializeField] private GameObject m_boxPrefab;

    [ContextMenu("Spawn Achievement boxes")]
    private void HandleSpawnBoxes()
    {
        for (int i = 0; i < m_boxes.Length; i++)
        {
            SpawnBoxes(m_boxes[i]);
        }
    }

    private void SpawnBoxes(BoxesToSpawn boxesToSpawn)
    {
        GameObject boxPrefab;
        int increasedReq = 0;
        for (int i = 0; i < boxesToSpawn.BoxesAmount; i++)
        {
            boxPrefab = Instantiate(m_boxPrefab, transform);
            string boxName = boxesToSpawn.BoxesName + " " + (i + 1);
            boxPrefab.name = boxName;
            AchievementBox box = boxPrefab.GetComponent<AchievementBox>();
            box.Initialize(boxName, boxesToSpawn.BoxesType,boxesToSpawn.BoxesInitialReq + increasedReq);
            increasedReq += boxesToSpawn.BoxesReqIncrease;
        }
    }


    [ContextMenu("---CAUTION---")]
    [ContextMenu("Clear Achievement boxes")]
    [ContextMenu("---CAUTION---")]
    private void ClearBoxes()
    {
        AchievementBox[] boxes = GetComponentsInChildren<AchievementBox>();
        foreach (AchievementBox box in boxes)
        {
            DestroyImmediate(box.gameObject);
        }
    }

    [Serializable]
    public struct BoxesToSpawn
    {
        [SerializeField] private string m_boxesName;
        [SerializeField] private int m_boxesAmount;
        [SerializeField] private AchievementType m_boxesType;
        [SerializeField] private int m_boxesInitialReq;
        [SerializeField] private int m_boxesReqIncrease;

        public readonly string BoxesName => m_boxesName;
        public readonly int BoxesAmount => m_boxesAmount;
        public readonly AchievementType BoxesType => m_boxesType;
        public readonly int BoxesInitialReq => m_boxesInitialReq;
        public readonly int BoxesReqIncrease => m_boxesReqIncrease;

    }
}
