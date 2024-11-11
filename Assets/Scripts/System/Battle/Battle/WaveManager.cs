using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static WaveManager;

public class WaveManager : MonoBehaviour
{
    // �V���O���g���C���X�^���X��ێ�����ÓI�ϐ�
    public static WaveManager Instance { get; private set; }

    [System.Serializable]
    public class SpawnPattern
    {
        public GameObject enemyPrefab; // ��������G�̃v���n�u
        public Transform spawnPoint; // ��������X�|�[���n�_
        public float spawnTime; // �������鎞�ԁi�b�j
    }

    [System.Serializable]
    public class Wave
    {
        public SpawnPattern[] spawnPatterns; // ���̃E�F�[�u�Ő�������p�^�[��
    }

    public Wave[] waves; // �����̃E�F�[�u
    public bool allWavesCompleted = false; // ���ׂẴE�F�[�u���I��������true�ɂ���t���O

    private int currentWaveIndex = 0; // ���݂̃E�F�[�u�̃C���f�b�N�X
    private int currentPatternIndex = 0; // ���݂̐����p�^�[���̃C���f�b�N�X
    private float timer = 0f; // �^�C�}�[
    private int enemiesLeftToSpawn = 0; // ���݂̃E�F�[�u�ł܂���������Ă��Ȃ��G�̐�

    public int totalSpawnPatternCount = 0; // SpawnPatterns�̑�����ێ�����ϐ�
    private int totalSpawnPatternCount_max;
    public int DeleteEnemycount = 0;

    public TextMeshProUGUI countenemy;

    

    // Awake���\�b�h�ŃV���O���g���C���X�^���X��ݒ�
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���Ԃł��̃I�u�W�F�N�g��j�����Ȃ��悤�ɂ���
        }
        else
        {
            Destroy(gameObject); // ���łɃC���X�^���X�����݂���ꍇ�A���̃I�u�W�F�N�g��j��
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
            totalSpawnPatternCount += wave.spawnPatterns.Length; // �eWave��spawnPatterns�z��̒��������v
        }
        totalSpawnPatternCount_max = totalSpawnPatternCount;
        countenemy.text = DeleteEnemycount.ToString() + "/" + totalSpawnPatternCount_max.ToString();
    }

    public void WaveManagerUpdate()
    {
        if (allWavesCompleted)
        {
            return; // ���ׂẴE�F�[�u���I�������牽�����Ȃ�
        }

        // ���݂̃E�F�[�u���������Ă��邩�m�F
        if (transform.childCount == 0 && enemiesLeftToSpawn == 0)
        {
            currentWaveIndex++; // ���̃E�F�[�u��
            if (currentWaveIndex >= waves.Length)
            {
                allWavesCompleted = true; // ���ׂẴE�F�[�u���I������
                Debug.Log("All waves completed!");
                GameFlowManager.Instance.Game_Clear();
                return;
            }
            else
            {
                currentPatternIndex = 0;
                enemiesLeftToSpawn = waves[currentWaveIndex].spawnPatterns.Length; // ���̃E�F�[�u�̓G����ݒ�
            }
        }

        // �^�C�}�[���X�V
        timer += Time.deltaTime;

        // �G�̐���
        if (enemiesLeftToSpawn > 0 && timer >= waves[currentWaveIndex].spawnPatterns[currentPatternIndex].spawnTime)
        {
            SpawnEnemy(waves[currentWaveIndex].spawnPatterns[currentPatternIndex].enemyPrefab, waves[currentWaveIndex].spawnPatterns[currentPatternIndex].spawnPoint);
            currentPatternIndex++;
            enemiesLeftToSpawn--;
            timer = 0f; // �^�C�}�[�����Z�b�g
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        // �G���w�肳�ꂽ�X�|�[���n�_�ɐ������A�e�I�u�W�F�N�g�i���̃I�u�W�F�N�g�j�ɐݒ�
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.transform.parent = this.transform; // ���������G�����̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɐݒ�
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
