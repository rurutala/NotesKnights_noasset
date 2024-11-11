using System.Collections.Generic;
using UnityEngine;

public class PowerDownPERitem: MonoBehaviour
{

    public float effect_time;
    public float effect_powerdown_percent;
    // 判定済みオブジェクトを格納するリスト
    private List<GameObject> triggeredObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // "Player"タグを持つオブジェクトを検知
        if (other.CompareTag("Enemy"))
        {
            // すでに判定されたオブジェクトかどうかを確認
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject); // オブジェクトをリストに追加
                ExecuteFunction(other.gameObject); // 関数を実行
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // "Player"タグを持つオブジェクトを検知
        if (other.CompareTag("Enemy"))
        {
            // すでに判定されたオブジェクトかどうかを確認
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject); // オブジェクトをリストに追加
                ExecuteFunction(other.gameObject); // 関数を実行
            }
        }
    }

    // 特定の関数を実行するメソッド
    private void ExecuteFunction(GameObject enemyObject)
    {
        // ここに実行したい処理を追加
        Debug.Log($"Function executed for: {enemyObject.name}");
        AttackDecreasepercentEffect effect = enemyObject.AddComponent<AttackDecreasepercentEffect>();
        effect.ApplyEffect(effect_time, effect_powerdown_percent);
    }
}
