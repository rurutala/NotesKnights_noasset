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

    // ����̊֐������s���郁�\�b�h
    private void ExecuteFunction()
    {
        // �����Ɏ��s������������ǉ�
        costspeedtransform costspeedtransform = CostManager.Instance.gameObject.AddComponent<costspeedtransform>();
        costspeedtransform.ApplyEffect(times_time,costtimes);
    }
}
