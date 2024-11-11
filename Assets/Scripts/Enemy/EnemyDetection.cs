using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    // すでに参照したオブジェクトを保持するためのHashSet
    private HashSet<GameObject> detectedEnemies = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // "Enemy"タグが付いたオブジェクトか確認
        if (other.CompareTag("Enemy"))
        {
            // 既に参照しているかどうかを確認
            if (!detectedEnemies.Contains(other.gameObject))
            {
                // 初めて参照されたオブジェクトの場合、HashSetに追加して処理を行う
                detectedEnemies.Add(other.gameObject);
                HandleEnemyDetected(other.gameObject);
            }
        }
    }

    // 敵が検出された時の処理
    private void HandleEnemyDetected(GameObject enemy)
    {
        // ここに敵が検出された時の処理を書く
        Debug.Log($"Enemy detected: {enemy.name}");
        GameFlowManager.Instance.EnemyInvasion();

    }
}
