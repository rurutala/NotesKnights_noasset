using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int unitID;
    public string unitName;
    public int unitcost;
    public float lifetime = 10f;  // ���j�b�g�����݂��鎞�ԁi�b�j

    public float elapsedTime = 0f;
    public bool isPlaced = false; // ���j�b�g���ݒu���ꂽ���ǂ���
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
        ResetUnitState(); // ���j�b�g��������ԂɃ��Z�b�g
    }

    public void UnitUpdate()
    {
        if (isPlaced)
        {
            // �o�ߎ��Ԃ��J�E���g
            elapsedTime += Time.deltaTime;

            // ���j�b�g�����Ԑ����𒴂����烊�Z�b�g
            if (elapsedTime >= lifetime)
            {
                unitPlacement.RemoveUnit(this);
                ResetUnitState();
                startCooltime();
            }
        }
        else
        {
            // ���j�b�g�̏�Ԃ�������
            elapsedTime = 0f;
            isPlaced = false;
            // ���̏��������K�v�ł���΂����ɒǉ�

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
        // ���j�b�g���{�ݒu���ꂽ���Ƃ𔻒�
        //this.gameObject.GetComponent<PlayerBattle>().ResetStats();
        this.gameObject.GetComponent<PlayerBattle>().isPlaced();
        isPlaced = true;
        DummyRange_normal.SetActive(false);
    }

    public void ResetUnitState()
    {

        this.gameObject.GetComponent<PlayerBattle>().ResetUnit();
        Debug.Log($"{unitName} ��������ԂɃ��Z�b�g����܂���");
    }
    public void MoveToNewPosition(Vector3 newPosition)
    {
        // �C�ӂ̈ʒu�Ɉړ�������
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
        correction_time = resttime_percent;//�P�ގ��ԂŁ�����
        Debug.Log(correction_time);
        coolTimecurrent = 0;
        in_cooltime = true;
    }
}
