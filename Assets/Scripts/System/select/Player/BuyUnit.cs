using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyUnit : MonoBehaviour
{
    public int unitID = -1; // キャラクターID

    // キャラクター情報
    public string unitName;
    public float hp, atk, def, speed, block, timeCost, buyCost;
    public string skillName, skillDetail ,wall;

    // UI要素
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
    // ボタンを有効化するメソッド
    public void EnableButton()
    {
        if (unitButton != null)
        {
            unitButton.interactable = true;
            lockpanel.SetActive(false);
        }
    }

    // ユニット選択時の処理
    public void OnUnitSelected()
    {
        if (unitID == -1 || UnitSelectManager.Instance.IsUnitSelected(unitID))
            return;

        UnitSelectManager.Instance.UnitsBuyDetailOn(unitID,unitName,this.GetComponent<BuyUnit>());
        UpdateUI();
    }

    // データをセットしてUIを更新
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

    // UIの更新
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
