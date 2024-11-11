using UnityEngine;
using System.Collections.Generic;

public class UnitSelectManager : MonoBehaviour
{
    public static UnitSelectManager Instance { get; private set; }

    public GameObject Unitsbuy;
    public GameObject chooseunit;

    public int currentselectunitid;
    public string currentselectunitname;

    public HoldUnit selectedHoldUnit; // ���ݑI������Ă���HoldUnit
    public List<HoldUnit> holdUnits = new List<HoldUnit>(); // HoldUnit�̃��X�g

    public BuyUnit selected_unit;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ���j�b�g�w����ʂ�\��
    public void UnitsBuyOn(HoldUnit holdUnit)
    {
        selectedHoldUnit = holdUnit; // �I�����ꂽHoldUnit��ۑ�
        Unitsbuy.SetActive(true);
        // �񖇖ڂ̉�ʂ�\���i�L�����N�^�[�I����ʁj
        // �����Ŋ��ɑI������Ă���L�����N�^�[�𖳌���
    }

    // �ڍ׉�ʂ�\��
    public void UnitsBuyDetailOn(int unitID, string name, BuyUnit unit)
    {
        selected_unit = unit;
        chooseunit.SetActive(true);
        currentselectunitid = unitID;
        currentselectunitname = name;
        // �O���ڂ̉�ʂ�\��
        // ���j�b�g�̏ڍׂ�\��
    }

    // �L�����N�^�[�I�����m�肵�p�[�e�B���X�V
    public void ConfirmUnitSelection()
    {
        int newID = currentselectunitid;
        // ���̃X���b�g�œ���ID���g���Ă����炻���������
        foreach (var holdUnit in holdUnits)
        {
            if (holdUnit.unitID == newID && holdUnit != selectedHoldUnit)
            {
                holdUnit.UpdateUnit(-1, null,null); // ID��-1�ɂ���UI���X�V
            }
        }

        // �I���X���b�g�ɐVID���Z�b�g
        selectedHoldUnit.UpdateUnit(newID, GetUnitSprite(newID),currentselectunitname); // UI���X�V

        // �p�[�e�B�����X�V
        UpdatePartyCharacterIDs();

        Unitsbuy.SetActive(false);
        chooseunit.SetActive(false);
    }
    // �p�[�e�B�L����ID���X�g��HoldUnit����X�V
    private void UpdatePartyCharacterIDs()
    {
        List<int> updatedPartyIDs = new List<int>(new int[holdUnits.Count]);

        for (int i = 0; i < holdUnits.Count; i++)
        {
            updatedPartyIDs[i] = holdUnits[i].unitID;
        }

        // DataManager�Ƀp�[�e�B�����X�V
        DataManager.Instance.UpdatePartyCharacterIDs(updatedPartyIDs);

    }


    // �w�肵��ID�����ɑI������Ă��邩�m�F
    public bool IsUnitSelected(int unitID)
    {
        return selectedHoldUnit.unitID == unitID;
    }

    // �w��ID�̃��j�b�g�摜���擾
    public Sprite GetUnitSprite(int unitID)
    {
        // "Resources/image/ui/Unit" + unitID�Ƃ����`���ŃX�v���C�g�����[�h
        string spritePath = "image/ui/Unit" + unitID;
        Sprite unitSprite = Resources.Load<Sprite>(spritePath);

        // �X�v���C�g��������Ȃ������ꍇ�̑Ώ�
        if (unitSprite == null)
        {
            Debug.LogError($"Sprite not found at path: {spritePath}");
            return null;
        }

        return unitSprite;
    }

    public void bought()
    {
        DataManager.Instance.moneyunit -= (int)selected_unit.buyCost;
        selected_unit.buyCost = 0;
    }

    public void UnitDataSend()
    {
        DataManager.Instance.SetHoldUnits(holdUnits);
    }
}
