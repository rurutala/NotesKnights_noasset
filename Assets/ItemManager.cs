using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // DataManagerからアイテムIDを取得し、プレハブを生成する
        foreach (int itemID in DataManager.Instance.boughtItemIDs)
        {
            // プレハブのパスを設定
            string prefabPath = "prefabs/item/item" + itemID;

            // プレハブをResourcesフォルダからロード
            GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);

            // プレハブが存在する場合のみ生成
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
                Debug.Log($"{prefabPath} が生成されました。");
            }
            else
            {
                Debug.LogError($"{prefabPath} のプレハブが見つかりませんでした。");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
