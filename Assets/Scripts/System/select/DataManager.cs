using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public int clearcount = 0;

    public int currentstage;
    public int nextcurrentstage;
    public int remaincount;
    public int money;
    public int moneyunit; 

    public List<GameObject> battleeffect;
    public List<int> partyCharacterIDs = new List<int>(new int[8]);
    public List<HoldUnit> holdUnits = new List<HoldUnit>();        // HoldUnit���X�g

    public bool event_on;
    public bool stageselected;

    public List<int> boughtItemIDs = new List<int>(); // �w���ς݃A�C�e����ID���X�g


    public PlayerDeck playerDeck = new PlayerDeck(); // �v���C���[�f�b�L

    public bool stage_nextup;

    private void Awake()
    {
        // �V���O���g���̃C���X�^���X���m��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͐V�����I�u�W�F�N�g��j��
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Battlestart()
    {
        foreach (var effect in battleeffect)
        {
            Instantiate(effect);
        }
    }

    // �p�[�e�B�L�����N�^�[ID�̍X�V
    public void UpdatePartyCharacterIDs(List<int> newPartyIDs)
    {
        partyCharacterIDs = new List<int>(newPartyIDs);
        Debug.Log("Party Character IDs Updated");
    }

    public void SetHoldUnits(List<HoldUnit> newHoldUnits)
    {
        holdUnits = new List<HoldUnit>(newHoldUnits);
        Debug.Log("HoldUnits list received and updated in DataManager");

        foreach (var holdUnit in holdUnits)
        {
            if (holdUnit.unitID != -1)
            {
                UnitData unitData = new UnitData
                {
                    unitID = holdUnit.unitID,
                    unitName = holdUnit.unitName,
                    unitIcon = Resources.Load<Sprite>($"image/ui/Unitbattle{holdUnit.unitID}")
                };

                if (unitData.unitIcon == null)
                {
                    Debug.LogError($"Unit icon not found for unitID: {holdUnit.unitID}");
                }

                playerDeck.units.Add(unitData);
            }
        }

        Debug.Log("PlayerDeck updated!");
    }

    public void RegisterBoughtItem(int itemID)
    {
        if (!boughtItemIDs.Contains(itemID))
        {
            boughtItemIDs.Add(itemID);
            Debug.Log($"Item {itemID} bought and registered.");
        }
    }
}
