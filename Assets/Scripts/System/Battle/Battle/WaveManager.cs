using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static WaveManager;

public class WaveManager : MonoBehaviour
{
    // シングルトンインスタンスを保持する静的変数
    public static WaveManager Instance { get; private set; }

    [System.Serializable]
    public class SpawnPattern
    {
        public GameObject enemyPrefab; // 生成する敵のプレハブ
        public Transform spawnPoint; // 生成するスポーン地点
        public float spawnTime; // 生成する時間（秒）
    }

    [System.Serializable]
    public class Wave
    {
        public SpawnPattern[] spawnPatterns; // そのウェーブで生成するパターン
    }

    public Wave[] waves; // 複数のウェーブ
    public bool allWavesCompleted = false; // すべてのウェーブが終了したらtrueにするフラグ

    private int currentWaveIndex = 0; // 現在のウェーブのインデックス
    private int currentPatternIndex = 0; // 現在の生成パターンのインデックス
    private float timer = 0f; // タイマー
    private int enemiesLeftToSpawn = 0; // 現在のウェーブでまだ生成されていない敵の数

    public int totalSpawnPatternCount = 0; // SpawnPatternsの総数を保持する変数
    private int totalSpawnPatternCount_max;
    public int DeleteEnemycount = 0;

    public TextMeshProUGUI countenemy;

    

    // Awakeメソッドでシングルトンインスタンスを設定
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン間でこのオブジェクトを破棄しないようにする
        }
        else
        {
            Destroy(gameObject); // すでにインスタンスが存在する場合、このオブジェクトを破棄
        }
    }

    void Start()
    {
        if (waves.Length > 0)
        {
            enemiesLeftToSpawn = waves[currentWaveIndex].spawnPatterns.Length;
        }
        foreach (Wave wave in waves)
        {
            totalSpawnPatternCount += wave.spawnPatterns.Length; // 各WaveのspawnPatterns配列の長さを合計
        }
        totalSpawnPatternCount_max = totalSpawnPatternCount;
        countenemy.text = DeleteEnemycount.ToString() + "/" + totalSpawnPatternCount_max.ToString();
    }

    public void WaveManagerUpdate()
    {
        if (allWavesCompleted)
        {
            return; // すべてのウェーブが終了したら何もしない
        }

        // 現在のウェーブが完了しているか確認
        if (transform.childCount == 0 && enemiesLeftToSpawn == 0)
        {
            currentWaveIndex++; // 次のウェーブへ
            if (currentWaveIndex >= waves.Length)
            {
                allWavesCompleted = true; // すべてのウェーブが終了した
                Debug.Log("All waves completed!");
                GameFlowManager.Instance.Game_Clear();
                return;
            }
            else
            {
                currentPatternIndex = 0;
                enemiesLeftToSpawn = waves[currentWaveIndex].spawnPatterns.Length; // 次のウェーブの敵数を設定
            }
        }

        // タイマーを更新
        timer += Time.deltaTime;

        // 敵の生成
        if (enemiesLeftToSpawn > 0 && timer >= waves[currentWaveIndex].spawnPatterns[currentPatternIndex].spawnTime)
        {
            SpawnEnemy(waves[currentWaveIndex].spawnPatterns[currentPatternIndex].enemyPrefab, waves[currentWaveIndex].spawnPatterns[currentPatternIndex].spawnPoint);
            currentPatternIndex++;
            enemiesLeftToSpawn--;
            timer = 0f; // タイマーをリセット
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        // 敵を指定されたスポーン地点に生成し、親オブジェクト（このオブジェクト）に設定
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.transform.parent = this.transform; // 生成した敵をこのスクリプトがアタッチされたオブジェクトの子オブジェクトに設定
        GameFlowManager.Instance.EnemyRegister(enemy);
    }
    
    public void DecreaseEnemycount()
    {
        totalSpawnPatternCount -= 1;
        countenemy.text = DeleteEnemycount.ToString() +"/" +  totalSpawnPatternCount_max.ToString();
    }

    public void DeleteEnemyCount()
    {
        DeleteEnemycount += 1;
        countenemy.text = DeleteEnemycount.ToString() + "/" + totalSpawnPatternCount_max.ToString();
    }
}
