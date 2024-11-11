using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public List<ItemData> itemList;                // ショップで使用するアイテムのリスト
    public List<ItemData> currentShopItems;        // ショップに表示されるアイテム
    public List<Button> itemButtons;               // アイテムボタンのリスト（3つ）
    public TMP_Text[] itemNames;                   // アイテム名のテキスト
    public TMP_Text[] itemMoney;
    public TMP_Text[] itemDescriptions;            // アイテム説明のテキスト
    public Image[] itemIcons;                      // アイテム画像のイメージ
    public int maxShopItems = 3;                   // ショップに表示するアイテム数

    private void Start()
    {
        SetRandomItems(); // ショップの初期設定
    }

    // ショップに表示するアイテムをランダムに選択する
    private void SetRandomItems()
    {
        currentShopItems.Clear(); // 現在のショップアイテムをクリア

        List<ItemData> availableItems = new List<ItemData>(itemList); // 利用可能なアイテムリストを作成

        // すでに購入済みのアイテムを除外
        foreach (int boughtItemID in DataManager.Instance.boughtItemIDs)
        {
            availableItems.RemoveAll(item => item.itemID == boughtItemID);
        }

        // ランダムにアイテムを選択
        for (int i = 0; i < maxShopItems; i++)
        {
            if (availableItems.Count == 0) break; // 選択可能なアイテムがない場合は終了

            int randomIndex = Random.Range(0, availableItems.Count);
            currentShopItems.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex);
        }

        UpdateShopUI(); // UIを更新
    }

    // ショップUIを更新する
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

                // ボタンのクリックイベントを設定
                int index = i; // ローカル変数を使ってキャプチャ
                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() => BuyItem(index));
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false); // 使用しないボタンを非表示
            }
        }
    }

    // アイテムを購入する処理
    private void BuyItem(int index)
    {
        if (currentShopItems[index].money <= DataManager.Instance.money) {
            ItemData selectedItem = currentShopItems[index];

            // DataManagerに購入したアイテムを登録
            DataManager.Instance.RegisterBoughtItem(selectedItem.itemID);

            // 購入後の処理（UI更新や再表示など）

            DataManager.Instance.money -= currentShopItems[index].money;
            //StageManger.Instance.Event_off();
            //StageManger.Instance.Stage_select_on();
        }
    }
}
