using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int unitID;
    public string unitName;
    public int unitcost;
    public float lifetime = 10f;  // ユニットが存在する時間（秒）

    public float elapsedTime = 0f;
    public bool isPlaced = false; // ユニットが設置されたかどうか
    public bool onWall;
    private IUnitPlacement unitPlacement;

    public GameObject DummyRange_normal;

    public bool in_cooltime;
    public float coolTimeMax;
    public float coolTimecurrent;

    public float correction_time = 1;

    //public Image reloadimage;

    private void Start()
    {
        unitPlacement = FindObjectOfType<UnitPlacementSystem>();
        ResetUnitState(); // ユニットを初期状態にリセット
    }

    public void UnitUpdate()
    {
        if (isPlaced)
        {
            // 経過時間をカウント
            elapsedTime += Time.deltaTime;

            // ユニットが時間制限を超えたらリセット
            if (elapsedTime >= lifetime)
            {
                unitPlacement.RemoveUnit(this);
                ResetUnitState();
                startCooltime();
            }
        }
        else
        {
            // ユニットの状態を初期化
            elapsedTime = 0f;
            isPlaced = false;
            // 他の初期化が必要であればここに追加

            coolTimecurrent += Time.deltaTime;
            if(coolTimecurrent > (coolTimeMax * correction_time))
            {
                coolTimecurrent = coolTimeMax;
                in_cooltime = false;
            }
            
        }
    }

    public void StartLifetimeCountdown()
    {
        // ユニットが本設置されたことを判定
        //this.gameObject.GetComponent<PlayerBattle>().ResetStats();
        this.gameObject.GetComponent<PlayerBattle>().isPlaced();
        isPlaced = true;
        DummyRange_normal.SetActive(false);
    }

    public void ResetUnitState()
    {

        this.gameObject.GetComponent<PlayerBattle>().ResetUnit();
        Debug.Log($"{unitName} が初期状態にリセットされました");
    }
    public void MoveToNewPosition(Vector3 newPosition)
    {
        // 任意の位置に移動させる
        transform.position = newPosition;
    }

    public void startCooltime()
    {
        correction_time = 1;
        coolTimecurrent = 0;
        in_cooltime = true;
    }

    public void startCooltime(float resttime_percent)
    {
        correction_time = resttime_percent;//撤退時間で％引き
        Debug.Log(correction_time);
        coolTimecurrent = 0;
        in_cooltime = true;
    }
}
