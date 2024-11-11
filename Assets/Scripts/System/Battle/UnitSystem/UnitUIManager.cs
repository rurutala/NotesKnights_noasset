using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitUIManager : MonoBehaviour
{
    public GameObject unitUIPrefab; // ユニットUIのプレハブ
    public Transform uiContainer;   // UIを配置するコンテナ

    public float xOffset = 100f;    // 各UIをx軸方向にどれだけずらすか

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

            // UIの位置を調整
            RectTransform rectTransform = unitUI.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(currentXPosition, rectTransform.anchoredPosition.y);

            // 次のUIをx軸方向にずらす
            currentXPosition -= xOffset;

            Debug.Log($"ユニットUIが生成されました: {unitData.unitName} (ID: {unitData.unitID})");
        }
    }
}
