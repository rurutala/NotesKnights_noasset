using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage_holl : MonoBehaviour
{
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
        // ぶつかったオブジェクトがEnemyタグを持っているか確認
        if (other.CompareTag("Enemy"))
        {
            // ぶつかったオブジェクトのEnemyBattleスクリプトを取得
            EnemyBattle enemyBattle = other.GetComponent<EnemyBattle>();

            // EnemyBattleスクリプトが見つかったらDieメソッドを呼び出す
            if (enemyBattle != null)
            {
                enemyBattle.Die();
                Debug.Log("Enemy died.");
            }
            else
            {
                Debug.LogError("EnemyBattle script not found on object with Enemy tag.");
            }
        }
    }
}
