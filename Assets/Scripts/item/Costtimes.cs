using System.Collections.Generic;
using UnityEngine;

public class Costtimesitem: MonoBehaviour
{

    public float costtimes;
    public float times_time;

    private void Start()
    {
        ExecuteFunction();
    }

    // 特定の関数を実行するメソッド
    private void ExecuteFunction()
    {
        // ここに実行したい処理を追加
        costspeedtransform costspeedtransform = CostManager.Instance.gameObject.AddComponent<costspeedtransform>();
        costspeedtransform.ApplyEffect(times_time,costtimes);
    }
}
