using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnitUI : MonoBehaviour
{
    public int unitID;
    public TextMeshProUGUI unitNameText;//��ʂɕ\������e�L�X�g
    public Image unitIconImage;//
    public Button unitButton;  // ���j�b�g�I���{�^��

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
        if (unit != null && !unit.isPlaced && CostManager.Instance.CanCost(unit.unitcost) && !unit.in_cooltime)//�g����Ƃ����g��Select�����悤�ɂ���
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
        // ���j�b�g�̏�Ԃ��Ď����ă{�^���̗L��/�����𐧌�
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
