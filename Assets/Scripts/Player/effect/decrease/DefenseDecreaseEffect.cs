using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDecreaseEffect : MonoBehaviour, IEffect
{
    public float modifierAmount;  // 攻撃力の増加量

    public void ApplyEffect(float duration,float modifierAmount)
    {
        this.modifierAmount = modifierAmount;

        StartCoroutine(RemoveEffectAfterDuration(duration));
    }

    private IEnumerator RemoveEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        // スクリプト自体を削除
        Destroy(this);
    }
}
