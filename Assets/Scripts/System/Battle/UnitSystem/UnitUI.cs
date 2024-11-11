using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitUI : MonoBehaviour
{
    public int unitID;
    public TextMeshProUGUI unitNameText;//画面に表示するテキスト
    public Image unitIconImage;//
    public Button unitButton;  // ユニット選択ボタン

    private Unit unit;

    public GameObject reload;
    public TextMeshProUGUI reload_text;
    public TextMeshProUGUI cost_text;

    public void SetUnitUI(string unitName, Sprite unitIcon)
    {
        unitNameText.text = unitName;
        unitIconImage.sprite = unitIcon;

        unit = FindUnitByID(unitID);
        cost_text.text = unit.unitcost.ToString();
    }

    public void OnClick()
    {
        if (unit != null && !unit.isPlaced && CostManager.Instance.CanCost(unit.unitcost) && !unit.in_cooltime)//使えるとき自身をSelectされるようにする
        {
            var selectionSystem = FindObjectOfType<UnitSelectionSystem>();
            selectionSystem.SelectUnitByID(unitID);
        }
    }

    private Unit FindUnitByID(int id)
    {
        Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit u in units)
        {
            if (u.unitID == id)
            {
                return u;
            }
        }
        return null;
    }

    private void Update()
    {
        // ユニットの状態を監視してボタンの有効/無効を制御
        if (unit != null)
        {
            reload.SetActive(false);
            unitButton.interactable = !unit.isPlaced && !GameFlowManager.Instance.game_stop && !unit.in_cooltime;

        }
        else
        {
           
        }
        if (!unit.isPlaced && !GameFlowManager.Instance.game_stop && unit.in_cooltime)
        {
            reload.SetActive(true);
            reload_text.text = ((unit.coolTimeMax * unit.correction_time) - unit.coolTimecurrent).ToString("f0");
        }
    }
}
