using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerBattle : MonoBehaviour
{
    public float max_HP = 100f;
    public float current_HP;  // ���݂�HP
    public float max_ATK = 20f;
    public float current_ATK;  // ���݂̍U����
    public float max_DEF = 10f;
    public float current_DEF;  // ���݂̖h���
    public float max_SPEED = 1.5f;  // �ő�U���Ԋu
    public float current_SPEED;  // ���݂̍U���Ԋu
    public int max_HOLD = 3;  // �ێ��ł���G�̍ő吔
    public int current_HOLD;  // ���ݕێ����Ă���G�̐�
    public float current_MP = 0;
    public float max_MP = 20;

    public List<GameObject> heldEnemies = new List<GameObject>(); // �ێ����Ă���G���X�g
    private float attackCooldown = 0f;

    public HashSet<GameObject> recognizedEnemies = new HashSet<GameObject>(); // �F���ς݂̓G��ێ�

    public Unit unit;

    public ISkill playerSkill;
    private IUnitPlacement unitPlacement;

    public Slider HP_slider;
    public Slider MP_slider;


    public player_audio playeraudio;

    private float lastClickTime = 0f;
    private float doubleClickTimeLimit = 0.18f; // �_�u���N���b�N�Ƃ݂Ȃ����ԊԊu

    void Start()
    {
        playeraudio = GetComponent<player_audio>();
        unitPlacement = FindObjectOfType<UnitPlacementSystem>();
        playerSkill = GetComponent<ISkill>();
        HP_slider.maxValue = max_HP;
        MP_slider.maxValue = max_MP;

        ResetStats();  // �X�^�[�g���Ƀp�����[�^�����Z�b�g
    }

    public void PlayerBattleUpdate()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            // �}�E�X�̈ʒu���烌�C���쐬
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ���ׂẴq�b�g���擾
            RaycastHit[] hits = Physics.RaycastAll(ray);

            // ���ׂẴq�b�g�����[�v���ď���
            foreach (RaycastHit hit in hits)
            {
                // �������g�Ƀq�b�g���������m�F
                if (hit.transform == transform && unit.isPlaced && current_MP == max_MP)
                {
                    // �q�I�u�W�F�N�g 'Clicked' ��T��
                    Transform child = transform.Find("Clicked");

                    if (child != null)
                    {
                        // ���ׂẴq�b�g���ēx���[�v���āA'Clicked'�Ƀq�b�g���������m�F
                        foreach (RaycastHit childHit in hits)
                        {
                            if (childHit.transform == child)
                            {
                                Debug.Log("�q�I�u�W�F�N�g 'Clicked' �Ƀq�b�g���܂����I");
                                // �����ɓ���̏�����ǉ�
                                if (playeraudio != null) playeraudio.skill_se();
                                playerSkill?.Activate(); // �X�L���𔭓�
                                current_MP = 0;
                                break; // �������̂Ń��[�v�𔲂���
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("�q�I�u�W�F�N�g 'Clicked' ��������܂���B");
                    }

                    break; // �������g���������̂Ń��[�v�𔲂���
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {                            
            // �_�u���N���b�N�����o���ꂽ�ꍇ
            if (Time.time - lastClickTime < doubleClickTimeLimit * Time.timeScale)
            {

                // �}�E�X�̈ʒu���烌�C���쐬
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // ���ׂẴq�b�g���擾
                RaycastHit[] hits = Physics.RaycastAll(ray);

                // ���ׂẴq�b�g�����[�v���ď���
                foreach (RaycastHit hit in hits)
                {
                    // �������g�Ƀq�b�g���������m�F
                    if (hit.transform == transform && unit.isPlaced)
                    {
                        // �q�I�u�W�F�N�g 'Clicked' ��T��
                        Transform child = transform.Find("Clicked");

                        if (child != null)
                        {
                            // ���ׂẴq�b�g���ēx���[�v���āA'Clicked'�Ƀq�b�g���������m�F
                            foreach (RaycastHit childHit in hits)
                            {
                                if (childHit.transform == child)
                                {
                                    Debug.Log("�q�I�u�W�F�N�g 'Clicked' �Ƀq�b�g���܂����I");
                                    // �����ɓ���̏�����ǉ�
                                    unit.startCooltime(unit.elapsedTime / unit.lifetime);
                                    escape();
                                    break; // �������̂Ń��[�v�𔲂���
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("�q�I�u�W�F�N�g 'Clicked' ��������܂���B");
                        }

                        break; // �������g���������̂Ń��[�v�𔲂���
                    }
                }
            }
            else
            {
            // 1��ڂ̃N���b�N���A�N���b�N������x�����s
                lastClickTime = Time.time;
            }
        }

        // �U���̃N�[���_�E�����I��������G���U��
        if (attackCooldown <= 0f && heldEnemies.Count > 0)
        {
            AttackHeldEnemy();
            attackCooldown = current_SPEED;
            // �q�I�u�W�F�N�g�ɂ��邷�ׂĂ�Animator���擾
            Animator[] childAnimators = GetComponentsInChildren<Animator>();

            // �擾����Animator���Ƃɏ���
            foreach (Animator animator in childAnimators)
            {
                if (animator != null)
                {
                    // 'Attack'�g���K�[��true�ɂ���
                    animator.SetTrigger("Attack");
                    Debug.Log("'Attack' �g���K�[���ݒ肳��܂����I");
                }
            }

            // Animator��������Ȃ������ꍇ
            if (childAnimators.Length == 0)
            {
                Debug.LogWarning("�q�I�u�W�F�N�g��Animator��������܂���ł����B");
            }
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }

        // HOLD�𒴂��Ă���G�����X�g����O���i�G���|���ꂽ�ꍇ�Ȃǁj
        heldEnemies.RemoveAll(enemy => enemy == null);
        if(heldEnemies.Count > cal_Defense(max_HOLD))
        {
            int decrease_hold = heldEnemies.Count - cal_block(max_HOLD);
            for(int i= 0;i < decrease_hold; i++)
            {
                recognizedEnemies.Remove(heldEnemies[heldEnemies.Count - i]);
                heldEnemies.RemoveAt(heldEnemies.Count - i); // �F���ς݃��X�g����폜
            }
        }

        if (unit.isPlaced)current_MP += Time.deltaTime;
        if (current_MP > max_MP) current_MP = max_MP;
        sliderupdate();
    }

    // �G�ւ̍U������
    private void AttackHeldEnemy()
    {
        if (heldEnemies.Count > 0)
        {
            GameObject enemy = heldEnemies[0]; // ���X�g�̍ŏ��̓G���U���ΏۂƂ���
            if (enemy != null)
            {
                if (playeraudio != null) playeraudio.attack_se();
                bool isEnemyDead = enemy.GetComponent<EnemyBattle>().TakeDamage(cal_Aattack(current_ATK)); // �G�Ƀ_���[�W��^���A���S���Ă��邩�m�F
                if (isEnemyDead)
                {
                    heldEnemies.Remove(enemy); // �G���|���ꂽ�烊�X�g����O��
                    Debug.Log("Enemy defeated and removed from held list.");
                }
            }
        }
    }

    // �v���C���[���_���[�W���󂯂鏈��
    public bool TakeDamage(float incomingDamage)
    {
        float damage = Mathf.Max(incomingDamage - cal_Defense(current_DEF), 0); // �_���[�W�v�Z
        current_HP -= damage;
        Debug.Log($"Player took {damage} damage. Remaining HP: {current_HP}");

        // HP��0�ȉ��Ȃ玀�S
        if (current_HP <= 0)
        {
            if (playeraudio != null) playeraudio.death_se();
            unit.startCooltime( unit.elapsedTime / unit.lifetime);
            ResetUnit();
            return true; // ���S��ԋp
        }

        return false; // ������
    }

    public void escape()
    {

        ResetUnit();
    }

    public void ResetUnit()
    {
        unitPlacement.RemoveUnit(this.gameObject.GetComponent<Unit>());
        this.gameObject.GetComponent<Unit>().elapsedTime = 0f;
        this.gameObject.GetComponent<Unit>().isPlaced = false;
        this.gameObject.GetComponent<Unit>().DummyRange_normal.SetActive(true);
        HP_slider.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        MP_slider.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        MoveToNewPosition(new Vector3(200, 0, 0)); // �V�����ʒu�Ɉړ�������i�����ł͗�Ƃ���(0, 0, 0)�j
        ResetStats(); // �X�e�[�^�X�����Z�b�g
        Debug.Log("Unit reset with full health and stats.");

    }

    // �p�����[�^���ő�l�Ƀ��Z�b�g
    public void ResetStats()
    {
        current_HP = max_HP;
        current_ATK = max_ATK;
        current_DEF = max_DEF;
        current_SPEED = max_SPEED;
        current_MP = 0;
        Debug.Log("Player stats reset to maximum values.");
        heldEnemies.Clear(); // �ێ����Ă���G���X�g�̏�����
        recognizedEnemies.Clear();
        // �I�u�W�F�N�g�ɃA�^�b�`����Ă��邷�ׂĂ�IEffect�R���|�[�l���g���擾
        IEffect[] effects = this.gameObject.GetComponents<IEffect>();

        // ���ׂĂ�IEffect�������폜
        foreach (IEffect effect in effects)
        {
            // �R���|�[�l���g�Ƃ��č폜����
            Destroy((MonoBehaviour)effect); // IEffect���p�����Ă���X�N���v�g��MonoBehaviour�Ȃ̂ŃL���X�g���K�v
        }
 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !recognizedEnemies.Contains(other.gameObject) && unit.isPlaced)
        {
            if (heldEnemies.Count < cal_block(max_HOLD))
            {
                heldEnemies.Add(other.gameObject);
                recognizedEnemies.Add(other.gameObject); // �F���ς݂̓G��ǉ�
                Debug.Log("Enemy held on trigger stay!");
            }
            else
            {
                // �U���̃N�[���_�E�����I��������G���U��
                if (attackCooldown <= 0f)
                {
                    var noheld = other.GetComponent<EnemyBattle>();
                    if (noheld != null)
                    {
                        noheld.TakeDamage(cal_Aattack(current_ATK));
                        if (playeraudio != null) playeraudio.attack_se();
                        attackCooldown = current_SPEED;
                        // �q�I�u�W�F�N�g�ɂ��邷�ׂĂ�Animator���擾
                        Animator[] childAnimators = GetComponentsInChildren<Animator>();

                        // �擾����Animator���Ƃɏ���
                        foreach (Animator animator in childAnimators)
                        {
                            if (animator != null)
                            {
                                // 'Attack'�g���K�[��true�ɂ���
                                animator.SetTrigger("Attack");
                                Debug.Log("'Attack' �g���K�[���ݒ肳��܂����I");
                            }
                        }

                        // Animator��������Ȃ������ꍇ
                        if (childAnimators.Length == 0)
                        {
                            Debug.LogWarning("�q�I�u�W�F�N�g��Animator��������܂���ł����B");
                        }
                    }
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            heldEnemies.Remove(other.gameObject);
            recognizedEnemies.Remove(other.gameObject); // �F���ς݃��X�g����폜
            Debug.Log("Enemy lost on trigger exit.");
        }
    }

    private void MoveToNewPosition(Vector3 newPosition)
    {
        // �C�ӂ̈ʒu�Ɉړ�������
        transform.position = newPosition;
    }

    private float cal_Aattack(float attack)
    {
        IEffect[] effects = this.GetComponents<IEffect>();

        float totalBonus = 0f;

        foreach (IEffect effect in effects)
        {
            // �U���͂Ɋ֘A������ʂ������v�Z����
            if (effect is AttackIncreaseEffect attackIncrease)
            {
                totalBonus += attackIncrease.modifierAmount;
            }
            else if (effect is AttackDecreaseEffect attackDecrease)
            {
                totalBonus -= attackDecrease.modifierAmount;
            }
        }
        attack += totalBonus;
        foreach (IEffect effect in effects)
        {
            // �U���͂Ɋ֘A������ʂ������v�Z����
            if (effect is AttackIncreasepercentEffect attackIncrease)
            {
                attack += (attack * (attackIncrease.modifierAmount / 100f));
            }
            else if (effect is AttackDecreasepercentEffect attackDecrease)
            {
                attack -= (attack * (attackDecrease.modifierAmount / 100f));
            }
        }

        return attack;
    }

    private float cal_Defense(float defense)
    {
        IEffect[] effects = this.GetComponents<IEffect>();

        float totalBonus = 0f;

        foreach (IEffect effect in effects)
        {
            // �U���͂Ɋ֘A������ʂ������v�Z����
            if (effect is DefenseIncreaseEffect defenseIncrease)
            {
                totalBonus += defenseIncrease.modifierAmount;
            }
            else if (effect is DefenseDecreaseEffect defenseDecrease)
            {
                totalBonus -= defenseDecrease.modifierAmount;
            }
        }
        defense += totalBonus;
        foreach (IEffect effect in effects)
        {
            // �U���͂Ɋ֘A������ʂ������v�Z����
            if (effect is DefenseIncreasepercentEffect defenseIncrease)
            {
                defense += (defense * (defenseIncrease.modifierAmount / 100f));
            }
            else if (effect is DefenseDecreasepercentEffect defenseDecrease)
            {
                defense -= (defense * (defenseDecrease.modifierAmount / 100f));
            }
        }

        return defense;
    }

    public int cal_block(float block)
    {
        IEffect[] effects = this.GetComponents<IEffect>();

        float totalBonus = 0f;

        foreach (IEffect effect in effects)
        {
            // �U���͂Ɋ֘A������ʂ������v�Z����
            if (effect is BlockIncreaseEffect defenseIncrease)
            {
                totalBonus += defenseIncrease.modifierAmount;
            }
            else if (effect is BlockDecreaseEffect defenseDecrease)
            {
                totalBonus -= defenseDecrease.modifierAmount;
            }
        }
        block += totalBonus;

        return (int)block;
    }

    public void isPlaced()
    {
        HP_slider.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
        MP_slider.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void sliderupdate()
    {
        HP_slider.value = current_HP;
        MP_slider.value = current_MP;
    }

}
