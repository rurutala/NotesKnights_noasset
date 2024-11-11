using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}

public class EnemyMovement : MonoBehaviour
{
    public int movementPattern; // パターンを表す整数
    public Transform[] waypoints; // 移動するための座標を格納する配列
    public float[] waypoints_waittime;
    private float current_waittime;
    public float speed = 2f; // 移動速度
    public Direction currentDirection; // 現在の移動方向を表す変数

    public int currentWaypointIndex = 0; // 現在の目標ウェイポイントのインデックス
    public bool isStopped = false; // 移動を停止しているかどうかを示すフラグ
    public float randomOffsetRange = 0.5f; // 座標に追加するランダムなオフセットの範囲

    private Vector3 randomOffset; // 現在のウェイポイントのランダムなオフセット
    public int randomSeed = 1234; // 乱数のシード値を設定

    private EnemyBattle enemybattle;
    [SerializeField] private EnemyAnim enemyAnim;
    private Rigidbody rb; // Rigidbodyを使用して移動

    private bool isright;

    void Start()
    {
        enemybattle = GetComponent<EnemyBattle>();
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得

        // 乱数のシードを設定
        Random.InitState(randomSeed);

        // 指定されたパターンに対応するオブジェクトを検索
        string patternObjectName = "points" + movementPattern;
        GameObject patternObject = GameObject.Find(patternObjectName);

        if (patternObject != null)
        {
            // 子オブジェクトをすべて取得してwaypointsに格納
            int childCount = patternObject.transform.childCount;
            waypoints = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                waypoints[i] = patternObject.transform.GetChild(i);
            }
        }
        else
        {
            Debug.LogError("Pattern object " + patternObjectName + " not found!");
        }

        // 最初のランダムなオフセットを設定
        SetRandomOffset();
    }

    public void EnemyMovementUpdate()
    {
        if (isStopped) return;

        if (currentWaypointIndex < waypoints.Length)
        {
            if (current_waittime < waypoints_waittime[currentWaypointIndex])
            {
                current_waittime += Time.deltaTime;
                enemyAnim.isWaiting();
                return;
            }

            // 現在のウェイポイントにランダムなオフセットを加えた位置をターゲットとする
            Vector3 targetPosition = waypoints[currentWaypointIndex].position + randomOffset;
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 向いている方向を検出
            DetectDirection(direction);

            // Rigidbodyを使って移動
            MoveTowardsTarget(direction);

            // 目標ウェイポイントに到達したかを確認
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                current_waittime = 0;
                currentWaypointIndex++;
                SetRandomOffset(); // 次のウェイポイントに進む際に新しいオフセットを設定
            }
        }
        else
        {
            enemyAnim.setDeath();
        }
    }

    void DetectDirection(Vector3 direction)
    {
        // X軸とZ軸の絶対値を比較して方向を判定
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            if (direction.x > 0)
            {
                if(!isright) enemyAnim.ChangeImageforDirection();
                currentDirection = Direction.RIGHT;
                isright = true;
                
            }
            else
            {
                if (isright) enemyAnim.ChangeImageforDirection();
                currentDirection = Direction.LEFT;
                isright = false;
            }
        }
        else
        {
            if (direction.z > 0)
            {
                currentDirection = Direction.UP;  // 前方向
            }
            else
            {
                currentDirection = Direction.DOWN;  // 後方向
            }
        }
    }

    // ランダムなオフセットを設定
    void SetRandomOffset()
    {
        randomOffset = new Vector3(
            Random.Range(-randomOffsetRange, randomOffsetRange),
            0, // Y軸方向にはオフセットをかけない
            Random.Range(-randomOffsetRange, randomOffsetRange)
        );
    }

    // Rigidbodyを使って目標に向かって移動
    void MoveTowardsTarget(Vector3 direction)
    {
        IEffect[] effects = this.GetComponents<IEffect>();
        foreach (IEffect effect in effects)
        {
            if (effect is StunEffect stun)
            {
                return;
            }

        }

        // 力を加えて敵を移動させる
        rb.MovePosition(rb.position + direction * cal_speed(speed) * Time.deltaTime);
        
    }

    public void changeStop(bool stop)
    {
        if (stop)
        {
            isStopped = true;
            enemyAnim.isWaiting();
        }
        else
        {
            isStopped = false;
            enemyAnim.isMoving();
        }
    }

    public float cal_speed(float speed)
    {
        IEffect[] effects = this.GetComponents<IEffect>();

        float totalBonus = 0f;

        foreach (IEffect effect in effects)
        {
            if (effect is MoveIncreaseEffect defenseIncrease)
            {
                totalBonus += defenseIncrease.modifierAmount;
            }
            else if (effect is MoveDecreaseEffect defenseDecrease)
            {
                totalBonus -= defenseDecrease.modifierAmount;
            }
        }
        speed += totalBonus;
        foreach (IEffect effect in effects)
        {
            if (effect is MoveIncreasepercentEffect defenseIncrease)
            {
                speed += (speed * (defenseIncrease.modifierAmount / 100f));
            }
            else if (effect is MoveDecreasepercentEffect defenseDecrease)
            {
                speed -= (speed * (defenseDecrease.modifierAmount / 100f));
            }
        }

        return speed;
    }
}
