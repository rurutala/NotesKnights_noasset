using System.Collections.Generic;
using UnityEngine;

public class PowerUPPERitem: MonoBehaviour
{

    public float effect_time;
    public float effect_powerup_percent;
    // 判定済みオブジェクトを格納するリスト
    public List<GameObject> triggeredObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // "Player"タグを持つオブジェクトを検知
        if (other.CompareTag("Player") || other.CompareTag("PlayerWall"))
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
        if (other.CompareTag("Player") || other.CompareTag("PlayerWall"))
        {
            // すでに判定されたオブジェクトかどうかを確認
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject); // オブジェクトをリストに追加
                ExecuteFunction(other.gameObject); // 関数を実行
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // "Player"タグを持つオブジェクトが範囲外に出たときにリストから削除
        if (other.CompareTag("Player") || other.CompareTag("PlayerWall"))
        {
            if (triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Remove(other.gameObject); // リストからオブジェクトを削除
                Debug.Log($"Object exited: {other.gameObject.name}");
            }
        }
    }

    // 特定の関数を実行するメソッド
    private void ExecuteFunction(GameObject playerObject)
    {
        // ここに実行したい処理を追加
        Debug.LogError($"Function executed for: {playerObject.name}");
        AttackIncreasepercentEffect effect = playerObject.AddComponent<AttackIncreasepercentEffect>();
        effect.ApplyEffect(effect_time, effect_powerup_percent);
    }
}
