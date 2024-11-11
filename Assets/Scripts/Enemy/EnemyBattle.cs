using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyBattle : MonoBehaviour
{
    public float HP = 100f;
    public float MAX_HP = 100f;
    public float ATK = 10f;
    public float DEF = 5f;
    public float SPEED = 1f;  // �U���Ԋu�𐧌䂷�邽�߂̃X�s�[�h


    public float safeDistance = 1.5f;

    public GameObject detectedPlayer; // ���m���ꂽ�v���C���[��ێ�
    private PlayerBattle playerBattle; // ���m���ꂽ�v���C���[��PlayerBattle�R���|�[�l���g
    private float attackCooldown = 0f;

    private EnemyMovement enemyMovement; // EnemyMovement���Q��
    public HashSet<GameObject> recognizedObjects = new HashSet<GameObject>(); // �F���ς݂̃I�u�W�F�N�g��ێ�
    public Slider HP_slider;
    public Slider MP_slider;
    [SerializeField]private EnemyAnim enemyAnim;
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        HP_slider.maxValue = MAX_HP;
    }

    public void EnemyBattleUpdate()
    {

        if (detectedPlayer != null)
        {
            if (attackCooldown <= 0f)
            {
                AttackPlayer();
                attackCooldown = SPEED;
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }

            if (playerBattle != null && playerBattle.current_HP <= 0)
            {
                detectedPlayer = null;
                playerBattle = null;
                enemyMovement.changeStop(false); // �v���C���[���|���ꂽ��ēx�ړ�������
            }
        }
        else{
            
            enemyMovement.changeStop(false); // �v���C���[����������ēx�ړ�������
        }
        HP_slider.value = HP;
    }

    // �v���C���[�ւ̍U������
    private void AttackPlayer()
    {
        if (playerBattle != null)
        {
            bool isPlayerDead = playerBattle.TakeDamage(cal_Aattack(ATK)); // �v���C���[�ɍU�����A���S���Ă��邩���m�F
            enemyAnim.setAttack();
            Debug.Log("Player attacked!");

            if (isPlayerDead)
            {
                recognizedObjects.Remove(playerBattle.gameObject);
                Debug.Log("Player defeated!");
                detectedPlayer = null;
                playerBattle = null;
                enemyMovement.changeStop(false); // �v���C���[���|���ꂽ��ړ����ĊJ

            }
        }

    }

    // �G���_���[�W���󂯂鏈��
    public bool TakeDamage(float incomingDamage)
    {
        float damage = Mathf.Max(incomingDamage - cal_Defense(DEF), 0); // �_���[�W�v�Z�i�h��͂������j
        HP -= damage;
        Debug.Log($"Enemy took {damage} damage. Remaining HP: {HP}");

        // HP��0�ȉ��Ȃ玀�S
        if (HP <= 0)
        {
            enemyAnim.setDeath();
            return true; // ���S��ԋp
        }

        return false; // ������
    }

    // �G���|��鏈��
    public void Die()
    {
        Debug.Log("Enemy died!");
        DisapperEnemy();
        Destroy(gameObject); // �G�I�u�W�F�N�g���폜
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.sharedMaterial == null)
        {
            return;
        }
        if (other.CompareTag("Player") && !recognizedObjects.Contains(other.gameObject) && other.sharedMaterial.name == "collision_material")
        {
            detectedPlayer = other.gameObject;
            playerBattle = detectedPlayer.GetComponent<PlayerBattle>();

            if (playerBattle != null && playerBattle.heldEnemies.Contains(this.gameObject) && detectedPlayer.GetComponent<Unit>().isPlaced)
            {
                Debug.Log("Player detected and Enemy stopped on trigger stay!");
                recognizedObjects.Add(other.gameObject); // �F���ς݂̃I�u�W�F�N�g��ǉ�
                enemyMovement.changeStop(true);
                float distanceToPlayer = Vector3.Distance(transform.position, detectedPlayer.transform.position);

                // ���S�ȋ�����ۂ��߂Ɉʒu���u���ɒ���
                if (distanceToPlayer < safeDistance)
                {
                    Vector3 directionAwayFromPlayer = (transform.position - detectedPlayer.transform.position).normalized;
                    transform.position = detectedPlayer.transform.position + directionAwayFromPlayer * safeDistance;

                    Debug.Log("Adjusted position to maintain safe distance from player.");
                }
            }
            else
            {
                Debug.Log("Player detected but not stopping, list full or player not found.");
                detectedPlayer = null;
                playerBattle = null;
            }



        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject == detectedPlayer && other.isTrigger == true)
        {
            detectedPlayer = null;
            recognizedObjects.Remove(other.gameObject); // �F���ς݃��X�g����폜
            enemyMovement.changeStop(false);
            Debug.Log("Player lost on trigger exit.");
        }
    }

    private bool hasCalledDisapperEnemy = false;
    private void DisapperEnemy()
    {
        // �������łɌĂ΂�Ă����牽�����Ȃ�
        if (hasCalledDisapperEnemy) return;

        // �t���O��true�ɐݒ�
        hasCalledDisapperEnemy = true;
        GameFlowManager.Instance.EnemyDelete(gameObject);

        // �G�̃J�E���g�����������鏈��
        WaveManager.Instance.DecreaseEnemycount();
        WaveManager.Instance.DeleteEnemyCount();
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
}
