using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    public float maxX = 10f; // x座標の最初の地点からの最大距離
    public float minX = -10f; // x座標の最初の地点からの最小距離
    public float maxZ = 10f; // z座標の最初の地点からの最大距離
    public float minZ = -10f; // z座標の最初の地点からの最小距離
    public float speed = 1f; // 移動速度

    public bool useX = true; // x軸を使用するかどうか
    public bool useZ = false; // z軸を使用するかどうか

    private Vector3 initialPosition; // オブジェクトの初期位置
    private bool movingPositive = true; // 正方向（右または上）に移動中かを示すフラグ

    void Start()
    {
        // オブジェクトの初期位置を記録
        initialPosition = transform.position;
    }

    void Update()
    {
        // 現在の位置を取得
        Vector3 currentPosition = transform.position;

        // x軸の移動
        if (useX)
        {
            if (movingPositive)
            {
                currentPosition.x += speed * Time.deltaTime; // 右に移動
                if (currentPosition.x >= initialPosition.x + maxX)
                {
                    movingPositive = false; // 最大値に達したら左に移動
                }
            }
            else
            {
                currentPosition.x -= speed * Time.deltaTime; // 左に移動
                if (currentPosition.x <= initialPosition.x + minX)
                {
                    movingPositive = true; // 最小値に達したら右に移動
                }
            }
        }

        // z軸の移動
        if (useZ)
        {
            if (movingPositive)
            {
                currentPosition.z += speed * Time.deltaTime; // 上に移動
                if (currentPosition.z >= initialPosition.z + maxZ)
                {
                    movingPositive = false; // 最大値に達したら下に移動
                }
            }
            else
            {
                currentPosition.z -= speed * Time.deltaTime; // 下に移動
                if (currentPosition.z <= initialPosition.z + minZ)
                {
                    movingPositive = true; // 最小値に達したら上に移動
                }
            }
        }

        // 新しい位置にオブジェクトを移動
        transform.position = currentPosition;
    }
}
