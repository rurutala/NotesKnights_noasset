using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerBattle : MonoBehaviour
{
    public float max_HP = 100f;
    public float current_HP;  // 現在のHP
    public float max_ATK = 20f;
    public float current_ATK;  // 現在の攻撃力
    public float max_DEF = 10f;
    public float current_DEF;  // 現在の防御力
    public float max_SPEED = 1.5f;  // 最大攻撃間隔
    public float current_SPEED;  // 現在の攻撃間隔
    public int max_HOLD = 3;  // 保持できる敵の最大数
    public int current_HOLD;  // 現在保持している敵の数
    public float current_MP = 0;
    public float max_MP = 20;

    public List<GameObject> heldEnemies = new List<GameObject>(); // 保持している敵リスト
    private float attackCooldown = 0f;

    public HashSet<GameObject> recognizedEnemies = new HashSet<GameObject>(); // 認識済みの敵を保持

    public Unit unit;

    public ISkill playerSkill;
    private IUnitPlacement unitPlacement;

    public Slider HP_slider;
    public Slider MP_slider;


    public player_audio playeraudio;

    private float lastClickTime = 0f;
    private float doubleClickTimeLimit = 0.18f; // ダブルクリックとみなす時間間隔

    void Start()
    {
        playeraudio = GetComponent<player_audio>();
        unitPlacement = FindObjectOfType<UnitPlacementSystem>();
        playerSkill = GetComponent<ISkill>();
        HP_slider.maxValue = max_HP;
        MP_slider.maxValue = max_MP;

        ResetStats();  // スタート時にパラメータをリセット
    }

    public void PlayerBattleUpdate()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            // マウスの位置からレイを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // すべてのヒットを取得
            RaycastHit[] hits = Physics.RaycastAll(ray);

            // すべてのヒットをループして処理
            foreach (RaycastHit hit in hits)
            {
                // 自分自身にヒットしたかを確認
                if (hit.transform == transform && unit.isPlaced && current_MP == max_MP)
                {
                    // 子オブジェクト 'Clicked' を探す
                    Transform child = transform.Find("Clicked");

                    if (child != null)
                    {
                        // すべてのヒットを再度ループして、'Clicked'にヒットしたかを確認
                        foreach (RaycastHit childHit in hits)
                        {
                            if (childHit.transform == child)
                            {
                                Debug.Log("子オブジェクト 'Clicked' にヒットしました！");
                                // ここに特定の処理を追加
                                if (playeraudio != null) playeraudio.skill_se();
                                playerSkill?.Activate(); // スキルを発動
                                current_MP = 0;
                                break; // 見つけたのでループを抜ける
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("子オブジェクト 'Clicked' が見つかりません。");
                    }

                    break; // 自分自身を見つけたのでループを抜ける
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {                            
            // ダブルクリックが検出された場合
            if (Time.time - lastClickTime < doubleClickTimeLimit * Time.timeScale)
            {

                // マウスの位置からレイを作成
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // すべてのヒットを取得
                RaycastHit[] hits = Physics.RaycastAll(ray);

                // すべてのヒットをループして処理
                foreach (RaycastHit hit in hits)
                {
                    // 自分自身にヒットしたかを確認
                    if (hit.transform == transform && unit.isPlaced)
                    {
                        // 子オブジェクト 'Clicked' を探す
                        Transform child = transform.Find("Clicked");

                        if (child != null)
                        {
                            // すべてのヒットを再度ループして、'Clicked'にヒットしたかを確認
                            foreach (RaycastHit childHit in hits)
                            {
                                if (childHit.transform == child)
                                {
                                    Debug.Log("子オブジェクト 'Clicked' にヒットしました！");
                                    // ここに特定の処理を追加
                                    unit.startCooltime(unit.elapsedTime / unit.lifetime);
                                    escape();
                                    break; // 見つけたのでループを抜ける
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("子オブジェクト 'Clicked' が見つかりません。");
                        }

                        break; // 自分自身を見つけたのでループを抜ける
                    }
                }
            }
            else
            {
            // 1回目のクリック時、クリック処理を遅延実行
                lastClickTime = Time.time;
            }
        }

        // 攻撃のクールダウンが終了したら敵を攻撃
        if (attackCooldown <= 0f && heldEnemies.Count > 0)
        {
            AttackHeldEnemy();
            attackCooldown = current_SPEED;
            // 子オブジェクトにあるすべてのAnimatorを取得
            Animator[] childAnimators = GetComponentsInChildren<Animator>();

            // 取得したAnimatorごとに処理
            foreach (Animator animator in childAnimators)
            {
                if (animator != null)
                {
                    // 'Attack'トリガーをtrueにする
                    animator.SetTrigger("Attack");
                    Debug.Log("'Attack' トリガーが設定されました！");
                }
            }

            // Animatorが見つからなかった場合
            if (childAnimators.Length == 0)
            {
                Debug.LogWarning("子オブジェクトにAnimatorが見つかりませんでした。");
            }
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }

        // HOLDを超えている敵をリストから外す（敵が倒された場合など）
        heldEnemies.RemoveAll(enemy => enemy == null);
        if(heldEnemies.Count > cal_Defense(max_HOLD))
        {
            int decrease_hold = heldEnemies.Count - cal_block(max_HOLD);
            for(int i= 0;i < decrease_hold; i++)
            {
                recognizedEnemies.Remove(heldEnemies[heldEnemies.Count - i]);
                heldEnemies.RemoveAt(heldEnemies.Count - i); // 認識済みリストから削除
            }
        }

        if (unit.isPlaced)current_MP += Time.deltaTime;
        if (current_MP > max_MP) current_MP = max_MP;
        sliderupdate();
    }

    // 敵への攻撃処理
    private void AttackHeldEnemy()
    {
        if (heldEnemies.Count > 0)
        {
            GameObject enemy = heldEnemies[0]; // リストの最初の敵を攻撃対象とする
            if (enemy != null)
            {
                if (playeraudio != null) playeraudio.attack_se();
                bool isEnemyDead = enemy.GetComponent<EnemyBattle>().TakeDamage(cal_Aattack(current_ATK)); // 敵にダメージを与え、死亡しているか確認
                if (isEnemyDead)
                {
                    heldEnemies.Remove(enemy); // 敵が倒されたらリストから外す
                    Debug.Log("Enemy defeated and removed from held list.");
                }
            }
        }
    }

    // プレイヤーがダメージを受ける処理
    public bool TakeDamage(float incomingDamage)
    {
        float damage = Mathf.Max(incomingDamage - cal_Defense(current_DEF), 0); // ダメージ計算
        current_HP -= damage;
        Debug.Log($"Player took {damage} damage. Remaining HP: {current_HP}");

        // HPが0以下なら死亡
        if (current_HP <= 0)
        {
            if (playeraudio != null) playeraudio.death_se();
            unit.startCooltime( unit.elapsedTime / unit.lifetime);
            ResetUnit();
            return true; // 死亡を返却
        }

        return false; // 生存中
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
        MoveToNewPosition(new Vector3(200, 0, 0)); // 新しい位置に移動させる（ここでは例として(0, 0, 0)）
        ResetStats(); // ステータスをリセット
        Debug.Log("Unit reset with full health and stats.");

    }

    // パラメータを最大値にリセット
    public void ResetStats()
    {
        current_HP = max_HP;
        current_ATK = max_ATK;
        current_DEF = max_DEF;
        current_SPEED = max_SPEED;
        current_MP = 0;
        Debug.Log("Player stats reset to maximum values.");
        heldEnemies.Clear(); // 保持している敵リストの初期化
        recognizedEnemies.Clear();
        // オブジェクトにアタッチされているすべてのIEffectコンポーネントを取得
        IEffect[] effects = this.gameObject.GetComponents<IEffect>();

        // すべてのIEffectを順次削除
        foreach (IEffect effect in effects)
        {
            // コンポーネントとして削除する
            Destroy((MonoBehaviour)effect); // IEffectを継承しているスクリプトはMonoBehaviourなのでキャストが必要
        }
 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !recognizedEnemies.Contains(other.gameObject) && unit.isPlaced)
        {
            if (heldEnemies.Count < cal_block(max_HOLD))
            {
                heldEnemies.Add(other.gameObject);
                recognizedEnemies.Add(other.gameObject); // 認識済みの敵を追加
                Debug.Log("Enemy held on trigger stay!");
            }
            else
            {
                // 攻撃のクールダウンが終了したら敵を攻撃
                if (attackCooldown <= 0f)
                {
                    var noheld = other.GetComponent<EnemyBattle>();
                    if (noheld != null)
                    {
                        noheld.TakeDamage(cal_Aattack(current_ATK));
                        if (playeraudio != null) playeraudio.attack_se();
                        attackCooldown = current_SPEED;
                        // 子オブジェクトにあるすべてのAnimatorを取得
                        Animator[] childAnimators = GetComponentsInChildren<Animator>();

                        // 取得したAnimatorごとに処理
                        foreach (Animator animator in childAnimators)
                        {
                            if (animator != null)
                            {
                                // 'Attack'トリガーをtrueにする
                                animator.SetTrigger("Attack");
                                Debug.Log("'Attack' トリガーが設定されました！");
                            }
                        }

                        // Animatorが見つからなかった場合
                        if (childAnimators.Length == 0)
                        {
                            Debug.LogWarning("子オブジェクトにAnimatorが見つかりませんでした。");
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
            recognizedEnemies.Remove(other.gameObject); // 認識済みリストから削除
            Debug.Log("Enemy lost on trigger exit.");
        }
    }

    private void MoveToNewPosition(Vector3 newPosition)
    {
        // 任意の位置に移動させる
        transform.position = newPosition;
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

    public int cal_block(float block)
    {
        IEffect[] effects = this.GetComponents<IEffect>();

        float totalBonus = 0f;

        foreach (IEffect effect in effects)
        {
            // 攻撃力に関連する効果だけを計算する
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
