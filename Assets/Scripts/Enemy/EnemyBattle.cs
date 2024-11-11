using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyBattle : MonoBehaviour
{
    public float HP = 100f;
    public float MAX_HP = 100f;
    public float ATK = 10f;
    public float DEF = 5f;
    public float SPEED = 1f;  // 攻撃間隔を制御するためのスピード


    public float safeDistance = 1.5f;

    public GameObject detectedPlayer; // 検知されたプレイヤーを保持
    private PlayerBattle playerBattle; // 検知されたプレイヤーのPlayerBattleコンポーネント
    private float attackCooldown = 0f;

    private EnemyMovement enemyMovement; // EnemyMovementを参照
    public HashSet<GameObject> recognizedObjects = new HashSet<GameObject>(); // 認識済みのオブジェクトを保持
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
                enemyMovement.changeStop(false); // プレイヤーが倒されたら再度移動を許可
            }
        }
        else{
            
            enemyMovement.changeStop(false); // プレイヤーが消えたら再度移動を許可
        }
        HP_slider.value = HP;
    }

    // プレイヤーへの攻撃処理
    private void AttackPlayer()
    {
        if (playerBattle != null)
        {
            bool isPlayerDead = playerBattle.TakeDamage(cal_Aattack(ATK)); // プレイヤーに攻撃し、死亡しているかを確認
            enemyAnim.setAttack();
            Debug.Log("Player attacked!");

            if (isPlayerDead)
            {
                recognizedObjects.Remove(playerBattle.gameObject);
                Debug.Log("Player defeated!");
                detectedPlayer = null;
                playerBattle = null;
                enemyMovement.changeStop(false); // プレイヤーが倒されたら移動を再開

            }
        }

    }

    // 敵がダメージを受ける処理
    public bool TakeDamage(float incomingDamage)
    {
        float damage = Mathf.Max(incomingDamage - cal_Defense(DEF), 0); // ダメージ計算（防御力を引く）
        HP -= damage;
        Debug.Log($"Enemy took {damage} damage. Remaining HP: {HP}");

        // HPが0以下なら死亡
        if (HP <= 0)
        {
            enemyAnim.setDeath();
            return true; // 死亡を返却
        }

        return false; // 生存中
    }

    // 敵が倒れる処理
    public void Die()
    {
        Debug.Log("Enemy died!");
        DisapperEnemy();
        Destroy(gameObject); // 敵オブジェクトを削除
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
                recognizedObjects.Add(other.gameObject); // 認識済みのオブジェクトを追加
                enemyMovement.changeStop(true);
                float distanceToPlayer = Vector3.Distance(transform.position, detectedPlayer.transform.position);

                // 安全な距離を保つために位置を瞬時に調整
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
            recognizedObjects.Remove(other.gameObject); // 認識済みリストから削除
            enemyMovement.changeStop(false);
            Debug.Log("Player lost on trigger exit.");
        }
    }

    private bool hasCalledDisapperEnemy = false;
    private void DisapperEnemy()
    {
        // もしすでに呼ばれていたら何もしない
        if (hasCalledDisapperEnemy) return;

        // フラグをtrueに設定
        hasCalledDisapperEnemy = true;
        GameFlowManager.Instance.EnemyDelete(gameObject);

        // 敵のカウントを減少させる処理
        WaveManager.Instance.DecreaseEnemycount();
        WaveManager.Instance.DeleteEnemyCount();
    }

    private float cal_Aattack(float attack)
    {
        IEffect[] effects = this.GetComponents<IEffect>();

        float totalBonus = 0f;

        foreach (IEffect effect in effects)
        {
            // 攻撃力に関連する効果だけを計算する
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
            // 攻撃力に関連する効果だけを計算する
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
            // 攻撃力に関連する効果だけを計算する
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
            // 攻撃力に関連する効果だけを計算する
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
