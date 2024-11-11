using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    // シングルトンインスタンスを保持する静的変数
    public static GameFlowManager Instance { get; private set; }

    private IUnitSelection unitSelection;
    private IUnitPlacement unitPlacement;
    private IUnitRotation unitRotation;

    private float lastClickTime = 0f;
    private float doubleClickTimeLimit = 0.18f; // ダブルクリックとみなす時間間隔

    public int TolerableEnemiesCount;
    public TextMeshProUGUI TolerableEnemiesText;

    private List<Unit> allUnits = new List<Unit>();//全Unitの参照(Updateを集約)
    public List<PlayerBattle> allplayerbattles = new List<PlayerBattle>();//全Unitの参照(Updateを集約)
    private List<EnemyBattle> allenemybattles = new List<EnemyBattle>();
    private List<EnemyMovement> allenemymovents = new List<EnemyMovement>();
    private bool isWaitingForDoubleClick = false;
    private Coroutine clickRoutine = null;

    public bool game_stop;
    public bool game_clear;
    public bool game_over;

    private void Awake()
    {
        // シングルトンのインスタンスを確立
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 既に存在する場合は新しいオブジェクトを破棄
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(null);
        AudioManager.Instance.PlaySE(AudioManager.Instance.seList[0]);
        unitSelection = FindObjectOfType<UnitSelectionSystem>();
        unitPlacement = FindObjectOfType<UnitPlacementSystem>();
        unitRotation = FindObjectOfType<UnitRotationSystem>();

        /*
        // Layerが"Player"のオブジェクトをすべて取得
        GameObject[] playerObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in playerObjects)
        {
            // レイヤーが"Player"であるか確認 (Layerを文字列で参照するためにはLayerMask.NameToLayerを使用)
            if (obj.layer == LayerMask.NameToLayer("Player"))
            {
                // Unitスクリプトがアタッチされているか確認
                Unit unit = obj.GetComponent<Unit>();
                if (unit != null)
                {
                    // Unitがアタッチされている場合、リストに追加
                    allUnits.Add(unit);
                }

            }
        }*/
    }

    private void Update()
    {
        if (game_stop) return;
        OtherUpdate();
        TolerableEnemiesText.text = TolerableEnemiesCount.ToString();


        if (unitSelection.GetSelectedUnit() != null)
        {
            /*
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;

                // ダブルクリックを検出
                if (Time.time - lastClickTime < doubleClickTimeLimit * Time.timeScale)
                {
                    // ダブルクリックで最終配置を決定
                    unitPlacement.ConfirmPlacement(unitSelection.GetSelectedUnit());
                    unitSelection.SelectUnitByID(-1); // 選択解除
                }
                else
                {
                    // シングルクリックの場合は仮配置
                    unitPlacement.PlaceUnit(unitSelection.GetSelectedUnit(), mousePosition);
                }

                lastClickTime = Time.time;
            }

            if (Input.GetMouseButtonDown(1)) // 右クリックで回転開始
            {
                unitRotation.StartRotation();
            }

            if (Input.GetMouseButtonUp(1)) // 右クリックを離した時に回転終了
            {
                unitRotation.EndRotation();
            }*/

            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePosition = Input.mousePosition;

                if (isWaitingForDoubleClick)
                {
                    // ダブルクリックが検出された場合
                    if (Time.time - lastClickTime < doubleClickTimeLimit * Time.timeScale)
                    {
                        // 2回目のクリックが一定時間内に発生したらダブルクリックとして処理
                        Debug.Log("Double click detected!");
                        if (clickRoutine != null)
                        {
                            StopCoroutine(clickRoutine); // クリック処理のコルーチンを止める
                        }
                        isWaitingForDoubleClick = false;
                        unitSelection.SelectUnitByID(-2); // 選択解除
                        TimeManager.Instance.TimeScaleChange(1f);
                    }
                }
                else
                {
                    // 1回目のクリック時、クリック処理を遅延実行
                    lastClickTime = Time.time;
                    isWaitingForDoubleClick = true;

                    // 一定時間待機してダブルクリックが発生しなければ通常のクリックを処理
                    clickRoutine = StartCoroutine(SingleClickRoutine(mousePosition));
                }
            }

            if (Input.GetMouseButtonDown(0)) // クリックで回転開始
            {
                Vector3 mousePosition = Input.mousePosition;
                // シングルクリックの場合は仮配置
                unitPlacement.PlaceUnit(unitSelection.GetSelectedUnit(), mousePosition);
                unitRotation.StartRotation();
            }

            if (Input.GetMouseButtonUp(0)) // クリックを離した時に回転終了
            {
                unitRotation.EndRotation();
            }
        }
    }

    private IEnumerator SingleClickRoutine(Vector3 mousePosition)
    {
        // 指定された時間だけ待機（ダブルクリックが発生するかどうかを待つ）
        yield return new WaitForSecondsRealtime(doubleClickTimeLimit);

        // ダブルクリックが発生しなかった場合はシングルクリックを処理
        if (isWaitingForDoubleClick)
        {
            // ダブルクリックで最終配置を決定
            unitPlacement.ConfirmPlacement(unitSelection.GetSelectedUnit());
            unitSelection.SelectUnitByID(-1); // 選択解除
            isWaitingForDoubleClick = false;
        }
    }
    public void Game_Clear()
    {
        AudioManager.Instance.PlayBGM(null);
        AudioManager.Instance.PlaySE(AudioManager.Instance.seList[1]);
        game_clear = true;
        GameFinish.Instance.GameClear();
        Game_Stop();
    }
    public void Game_Over()
    {
        AudioManager.Instance.PlayBGM(null);
        AudioManager.Instance.PlaySE(AudioManager.Instance.seList[2]);
        game_over = true;
        GameFinish.Instance.GameOver();
        Game_Stop();
    }

    public void Game_Stop()
    {
        game_stop = true;
    }

    public void Game_Start()
    {
        game_stop = false;
    }

    private void OtherUpdate()
    {
        WaveManager.Instance.WaveManagerUpdate();
        CostManager.Instance.CostUpdate();

        // 各UnitのUnitUpdate()を呼び出す
        foreach (Unit unit in allUnits)
        {
            unit.UnitUpdate();
        }

        foreach (PlayerBattle playerbattle in allplayerbattles)
        {
            playerbattle.PlayerBattleUpdate();
        }

        if (allenemybattles.Count != 0)
        {
            foreach (EnemyBattle enemybattle in allenemybattles)
            {
                enemybattle.EnemyBattleUpdate();
            }
        }
        if (allenemymovents.Count != 0)
        {
            foreach (EnemyMovement enemymovement in allenemymovents)
            {
                enemymovement.EnemyMovementUpdate();
            }
        }
    }
    public void EnemyInvasion()
    {
        if (TolerableEnemiesCount == 0) return;

        TolerableEnemiesCount -= 1;
        TolerableEnemiesText.text = TolerableEnemiesCount.ToString();
        if(TolerableEnemiesCount == 0)
        {
            Game_Over();
        }
    }

    public void EnemyRegister(GameObject enemy)
    {
        allenemybattles.Add(enemy.GetComponent<EnemyBattle>());
        allenemymovents.Add(enemy.GetComponent<EnemyMovement>());
    }

    public void EnemyDelete(GameObject enemyToRemove)
    {
        allenemybattles.RemoveAll(enemy => enemy.gameObject == enemyToRemove);
        allenemymovents.RemoveAll(enemy => enemy.gameObject == enemyToRemove);
    }

    public void PlayerRegister(GameObject player)
    {
        allplayerbattles.Add(player.GetComponent<PlayerBattle>());
        allUnits.Add(player.GetComponent<Unit>());
    }
}
