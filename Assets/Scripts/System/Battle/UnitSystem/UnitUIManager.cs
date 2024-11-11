using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitUIManager : MonoBehaviour
{
    public GameObject unitUIPrefab; // ���j�b�gUI�̃v���n�u
    public Transform uiContainer;   // UI��z�u����R���e�i

    public float xOffset = 100f;    // �eUI��x�������ɂǂꂾ�����炷��

    private void Start()
    {
        var player = FindObjectOfType<Player>();
        if (player != null)
        {
            GenerateUnitUI(player.playerDeck);
        }
    }

    private void GenerateUnitUI(PlayerDeck deck)
    {
        float currentXPosition = 0f;

        foreach (var unitData in deck.units)
        {
            GameObject unitUI = Instantiate(unitUIPrefab, uiContainer);
            UnitUI unitUIScript = unitUI.GetComponent<UnitUI>();

            unitUIScript.unitID = unitData.unitID;
            unitUIScript.SetUnitUI(unitData.unitName, unitData.unitIcon);

            // UI�̈ʒu�𒲐�
            RectTransform rectTransform = unitUI.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(currentXPosition, rectTransform.anchoredPosition.y);

            // ����UI��x�������ɂ��炷
            currentXPosition -= xOffset;

            Debug.Log($"���j�b�gUI����������܂���: {unitData.unitName} (ID: {unitData.unitID})");
        }
    }
}
