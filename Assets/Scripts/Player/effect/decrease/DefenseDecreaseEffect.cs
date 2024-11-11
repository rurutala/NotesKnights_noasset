using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDecreaseEffect : MonoBehaviour, IEffect
{
    public float modifierAmount;  // �U���͂̑�����

    public void ApplyEffect(float duration,float modifierAmount)
    {
        this.modifierAmount = modifierAmount;

        StartCoroutine(RemoveEffectAfterDuration(duration));
    }

    private IEnumerator RemoveEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        // �X�N���v�g���̂��폜
        Destroy(this);
    }
}
