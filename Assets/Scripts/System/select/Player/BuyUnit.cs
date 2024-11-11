using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyUnit : MonoBehaviour
{
    public int unitID = -1; // �L�����N�^�[ID

    // �L�����N�^�[���
    public string unitName;
    public float hp, atk, def, speed, block, timeCost, buyCost;
    public string skillName, skillDetail ,wall;

    // UI�v�f
    public TMP_Text unitNameText, hpText, atkText, defText, speedText, wallText, blockText;
    public TMP_Text timeCostText, buyCostText, skillNameText, skillDetailText;
    public Button unitButton;
    public GameObject lockpanel;
    public GameObject Unitbuy;
    public lockbuy _lockbuy;
    public Image chooseUnit;

    private void Start()
    {
        unitButton.onClick.AddListener(OnUnitSelected);
    }

    private void Update()
    {
        if (unitID == -1 || UnitSelectManager.Instance.IsUnitSelected(unitID))
        {
            unitButton.interactable = false;
            lockpanel.SetActive(true);
        }

    }
    // �{�^����L�������郁�\�b�h
    public void EnableButton()
    {
        if (unitButton != null)
        {
            unitButton.interactable = true;
            lockpanel.SetActive(false);
        }
    }

    // ���j�b�g�I�����̏���
    public void OnUnitSelected()
    {
        if (unitID == -1 || UnitSelectManager.Instance.IsUnitSelected(unitID))
            return;

        UnitSelectManager.Instance.UnitsBuyDetailOn(unitID,unitName,this.GetComponent<BuyUnit>());
        UpdateUI();
    }

    // �f�[�^���Z�b�g����UI���X�V
    public void SetBuyUnitData(
        int id, string name, float hp, float atk, float def, float speed,
        string wall, float block, float timeCost, float buyCost, string skillName, string skillDetail)
    {
        unitID = id;
        unitName = name;
        this.hp = hp; this.atk = atk; this.def = def;
        this.speed = speed; this.wall = wall; this.block = block;
        this.timeCost = timeCost; this.buyCost = buyCost;
        this.skillName = skillName; this.skillDetail = skillDetail;

        UpdateUI();
    }

    // UI�̍X�V
    private void UpdateUI()
    {
        unitNameText.text = unitName;
        hpText.text = hp.ToString();
        atkText.text = atk.ToString();
        defText.text = def.ToString();
        speedText.text = speed.ToString();
        wallText.text = wall.ToString();
        blockText.text = block.ToString();
        timeCostText.text = timeCost.ToString();
        buyCostText.text = buyCost.ToString();
        skillNameText.text = skillName;
        skillDetailText.text = skillDetail;
        _lockbuy.updatecost((int)buyCost);
        chooseUnit.sprite = UnitSelectManager.Instance.GetUnitSprite(UnitSelectManager.Instance.currentselectunitid);
    }
}
