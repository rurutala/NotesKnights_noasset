using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public List<ItemData> itemList;                // �V���b�v�Ŏg�p����A�C�e���̃��X�g
    public List<ItemData> currentShopItems;        // �V���b�v�ɕ\�������A�C�e��
    public List<Button> itemButtons;               // �A�C�e���{�^���̃��X�g�i3�j
    public TMP_Text[] itemNames;                   // �A�C�e�����̃e�L�X�g
    public TMP_Text[] itemMoney;
    public TMP_Text[] itemDescriptions;            // �A�C�e�������̃e�L�X�g
    public Image[] itemIcons;                      // �A�C�e���摜�̃C���[�W
    public int maxShopItems = 3;                   // �V���b�v�ɕ\������A�C�e����

    private void Start()
    {
        SetRandomItems(); // �V���b�v�̏����ݒ�
    }

    // �V���b�v�ɕ\������A�C�e���������_���ɑI������
    private void SetRandomItems()
    {
        currentShopItems.Clear(); // ���݂̃V���b�v�A�C�e�����N���A

        List<ItemData> availableItems = new List<ItemData>(itemList); // ���p�\�ȃA�C�e�����X�g���쐬

        // ���łɍw���ς݂̃A�C�e�������O
        foreach (int boughtItemID in DataManager.Instance.boughtItemIDs)
        {
            availableItems.RemoveAll(item => item.itemID == boughtItemID);
        }

        // �����_���ɃA�C�e����I��
        for (int i = 0; i < maxShopItems; i++)
        {
            if (availableItems.Count == 0) break; // �I���\�ȃA�C�e�����Ȃ��ꍇ�͏I��

            int randomIndex = Random.Range(0, availableItems.Count);
            currentShopItems.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex);
        }

        UpdateShopUI(); // UI���X�V
    }

    // �V���b�vUI���X�V����
    private void UpdateShopUI()
    {
        for (int i = 0; i < maxShopItems; i++)
        {
            if (i < currentShopItems.Count)
            {
                itemButtons[i].gameObject.SetActive(true);
                itemNames[i].text = currentShopItems[i].itemName;
                itemDescriptions[i].text = currentShopItems[i].itemEffect;
                itemMoney[i].text = currentShopItems[i].money.ToString();
                itemIcons[i].sprite = currentShopItems[i].itemIcon;

                // �{�^���̃N���b�N�C�x���g��ݒ�
                int index = i; // ���[�J���ϐ����g���ăL���v�`��
                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() => BuyItem(index));
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false); // �g�p���Ȃ��{�^�����\��
            }
        }
    }

    // �A�C�e�����w�����鏈��
    private void BuyItem(int index)
    {
        if (currentShopItems[index].money <= DataManager.Instance.money) {
            ItemData selectedItem = currentShopItems[index];

            // DataManager�ɍw�������A�C�e����o�^
            DataManager.Instance.RegisterBoughtItem(selectedItem.itemID);

            // �w����̏����iUI�X�V��ĕ\���Ȃǁj

            DataManager.Instance.money -= currentShopItems[index].money;
            //StageManger.Instance.Event_off();
            //StageManger.Instance.Stage_select_on();
        }
    }
}
