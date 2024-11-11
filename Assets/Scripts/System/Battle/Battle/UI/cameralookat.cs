using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameralookat : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 自分自身の位置からのオフセット
        Vector3 offset = new Vector3(0, 20, -30);

        // オフセット位置を計算
        Vector3 targetPosition = transform.position + offset;

        // 計算した位置を向く
        transform.LookAt(targetPosition);
    }
}
