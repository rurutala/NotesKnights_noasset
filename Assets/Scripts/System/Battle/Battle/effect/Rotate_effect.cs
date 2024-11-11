using UnityEngine;

public class Rotate_effect : MonoBehaviour
{
    public float speed = 10f; // 回転速度
    public bool clockwise = true; // 時計回りかどうかを決めるフラグ

    public bool rotateX = false; // x軸で回転するかどうか
    public bool rotateY = false; // y軸で回転するかどうか
    public bool rotateZ = false; // z軸で回転するかどうか

    void Update()
    {
        // 回転方向を決定 (clockwise: 時計回り、反時計回り)
        float rotationDirection = clockwise ? 1f : -1f;

        // 回転量を計算するためのベクトルを作成
        Vector3 rotationVector = Vector3.zero;

        // 各軸に対して回転するかどうかをチェック
        if (rotateX)
        {
            rotationVector.x = rotationDirection;
        }
        if (rotateY)
        {
            rotationVector.y = rotationDirection;
        }
        if (rotateZ)
        {
            rotationVector.z = rotationDirection;
        }

        // 回転させる
        transform.Rotate(rotationVector * speed * Time.deltaTime);
    }
}
