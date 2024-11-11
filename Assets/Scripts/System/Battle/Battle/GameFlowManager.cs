using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    // �V���O���g���C���X�^���X��ێ�����ÓI�ϐ�
    public static GameFlowManager Instance { get; private set; }

    private IUnitSelection unitSelection;
    private IUnitPlacement unitPlacement;
    private IUnitRotation unitRotation;

    private float lastClickTime = 0f;
    private float doubleClickTimeLimit = 0.18f; // �_�u���N���b�N�Ƃ݂Ȃ����ԊԊu

    public int TolerableEnemiesCount;
    public TextMeshProUGUI TolerableEnemiesText;

    private List<Unit> allUnits = new List<Unit>();//�SUnit�̎Q��(Update���W��)
    public List<PlayerBattle> allplayerbattles = new List<PlayerBattle>();//�SUnit�̎Q��(Update���W��)
    private List<EnemyBattle> allenemybattles = new List<EnemyBattle>();
    private List<EnemyMovement> allenemymovents = new List<EnemyMovement>();
    private bool isWaitingForDoubleClick = false;
    private Coroutine clickRoutine = null;

    public bool game_stop;
    public bool game_clear;
    public bool game_over;

    private void Awake()
    {
        // �V���O���g���̃C���X�^���X���m��
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͐V�����I�u�W�F�N�g��j��
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
        // Layer��"Player"�̃I�u�W�F�N�g�����ׂĎ擾
        GameObject[] playerObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in playerObjects)
        {
            // ���C���[��"Player"�ł��邩�m�F (Layer�𕶎���ŎQ�Ƃ��邽�߂ɂ�LayerMask.NameToLayer���g�p)
            if (obj.layer == LayerMask.NameToLayer("Player"))
            {
                // Unit�X�N���v�g���A�^�b�`����Ă��邩�m�F
                Unit unit = obj.GetComponent<Unit>();
                if (unit != null)
                {
                    // Unit���A�^�b�`����Ă���ꍇ�A���X�g�ɒǉ�
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

                // �_�u���N���b�N�����o
                if (Time.time - lastClickTime < doubleClickTimeLimit * Time.timeScale)
                {
                    // �_�u���N���b�N�ōŏI�z�u������
                    unitPlacement.ConfirmPlacement(unitSelection.GetSelectedUnit());
                    unitSelection.SelectUnitByID(-1); // �I������
                }
                else
                {
                    // �V���O���N���b�N�̏ꍇ�͉��z�u
                    unitPlacement.PlaceUnit(unitSelection.GetSelectedUnit(), mousePosition);
                }

                lastClickTime = Time.time;
            }

            if (Input.GetMouseButtonDown(1)) // �E�N���b�N�ŉ�]�J�n
            {
                unitRotation.StartRotation();
            }

            if (Input.GetMouseButtonUp(1)) // �E�N���b�N�𗣂������ɉ�]�I��
            {
                unitRotation.EndRotation();
            }*/

            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePosition = Input.mousePosition;

                if (isWaitingForDoubleClick)
                {
                    // �_�u���N���b�N�����o���ꂽ�ꍇ
                    if (Time.time - lastClickTime < doubleClickTimeLimit * Time.timeScale)
                    {
                        // 2��ڂ̃N���b�N����莞�ԓ��ɔ���������_�u���N���b�N�Ƃ��ď���
                        Debug.Log("Double click detected!");
                        if (clickRoutine != null)
                        {
                            StopCoroutine(clickRoutine); // �N���b�N�����̃R���[�`�����~�߂�
                        }
                        isWaitingForDoubleClick = false;
                        unitSelection.SelectUnitByID(-2); // �I������
                        TimeManager.Instance.TimeScaleChange(1f);
                    }
                }
                else
                {
                    // 1��ڂ̃N���b�N���A�N���b�N������x�����s
                    lastClickTime = Time.time;
                    isWaitingForDoubleClick = true;

                    // ��莞�ԑҋ@���ă_�u���N���b�N���������Ȃ���Βʏ�̃N���b�N������
                    clickRoutine = StartCoroutine(SingleClickRoutine(mousePosition));
                }
            }

            if (Input.GetMouseButtonDown(0)) // �N���b�N�ŉ�]�J�n
            {
                Vector3 mousePosition = Input.mousePosition;
                // �V���O���N���b�N�̏ꍇ�͉��z�u
                unitPlacement.PlaceUnit(unitSelection.GetSelectedUnit(), mousePosition);
                unitRotation.StartRotation();
            }

            if (Input.GetMouseButtonUp(0)) // �N���b�N�𗣂������ɉ�]�I��
            {
                unitRotation.EndRotation();
            }
        }
    }

    private IEnumerator SingleClickRoutine(Vector3 mousePosition)
    {
        // �w�肳�ꂽ���Ԃ����ҋ@�i�_�u���N���b�N���������邩�ǂ�����҂j
        yield return new WaitForSecondsRealtime(doubleClickTimeLimit);

        // �_�u���N���b�N���������Ȃ������ꍇ�̓V���O���N���b�N������
        if (isWaitingForDoubleClick)
        {
            // �_�u���N���b�N�ōŏI�z�u������
            unitPlacement.ConfirmPlacement(unitSelection.GetSelectedUnit());
            unitSelection.SelectUnitByID(-1); // �I������
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

        // �eUnit��UnitUpdate()���Ăяo��
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
