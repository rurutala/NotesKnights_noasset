using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIncreaseEffect : MonoBehaviour, IEffect
{
    public int modifierAmount;  // 攻撃力の増加量

    public void ApplyEffect(float duration,float modifierAmount)
    {
        this.modifierAmount = (int)modifierAmount;

        StartCoroutine(RemoveEffectAfterDuration(duration));
    }

    private IEnumerator RemoveEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        // スクリプト自体を削除
        Destroy(this);
    }
}
