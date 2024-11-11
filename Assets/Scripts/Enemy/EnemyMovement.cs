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
    public int movementPattern; // �p�^�[����\������
    public Transform[] waypoints; // �ړ����邽�߂̍��W���i�[����z��
    public float[] waypoints_waittime;
    private float current_waittime;
    public float speed = 2f; // �ړ����x
    public Direction currentDirection; // ���݂̈ړ�������\���ϐ�

    public int currentWaypointIndex = 0; // ���݂̖ڕW�E�F�C�|�C���g�̃C���f�b�N�X
    public bool isStopped = false; // �ړ����~���Ă��邩�ǂ����������t���O
    public float randomOffsetRange = 0.5f; // ���W�ɒǉ����郉���_���ȃI�t�Z�b�g�͈̔�

    private Vector3 randomOffset; // ���݂̃E�F�C�|�C���g�̃����_���ȃI�t�Z�b�g
    public int randomSeed = 1234; // �����̃V�[�h�l��ݒ�

    private EnemyBattle enemybattle;
    [SerializeField] private EnemyAnim enemyAnim;
    private Rigidbody rb; // Rigidbody���g�p���Ĉړ�

    private bool isright;

    void Start()
    {
        enemybattle = GetComponent<EnemyBattle>();
        rb = GetComponent<Rigidbody>(); // Rigidbody���擾

        // �����̃V�[�h��ݒ�
        Random.InitState(randomSeed);

        // �w�肳�ꂽ�p�^�[���ɑΉ�����I�u�W�F�N�g������
        string patternObjectName = "points" + movementPattern;
        GameObject patternObject = GameObject.Find(patternObjectName);

        if (patternObject != null)
        {
            // �q�I�u�W�F�N�g�����ׂĎ擾����waypoints�Ɋi�[
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

        // �ŏ��̃����_���ȃI�t�Z�b�g��ݒ�
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

            // ���݂̃E�F�C�|�C���g�Ƀ����_���ȃI�t�Z�b�g���������ʒu���^�[�Q�b�g�Ƃ���
            Vector3 targetPosition = waypoints[currentWaypointIndex].position + randomOffset;
            Vector3 direction = (targetPosition - transform.position).normalized;

            // �����Ă�����������o
            DetectDirection(direction);

            // Rigidbody���g���Ĉړ�
            MoveTowardsTarget(direction);

            // �ڕW�E�F�C�|�C���g�ɓ��B���������m�F
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                current_waittime = 0;
                currentWaypointIndex++;
                SetRandomOffset(); // ���̃E�F�C�|�C���g�ɐi�ލۂɐV�����I�t�Z�b�g��ݒ�
            }
        }
        else
        {
            enemyAnim.setDeath();
        }
    }

    void DetectDirection(Vector3 direction)
    {
        // X����Z���̐�Βl���r���ĕ����𔻒�
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
                currentDirection = Direction.UP;  // �O����
            }
            else
            {
                currentDirection = Direction.DOWN;  // �����
            }
        }
    }

    // �����_���ȃI�t�Z�b�g��ݒ�
    void SetRandomOffset()
    {
        randomOffset = new Vector3(
            Random.Range(-randomOffsetRange, randomOffsetRange),
            0, // Y�������ɂ̓I�t�Z�b�g�������Ȃ�
            Random.Range(-randomOffsetRange, randomOffsetRange)
        );
    }

    // Rigidbody���g���ĖڕW�Ɍ������Ĉړ�
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

        // �͂������ēG���ړ�������
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
