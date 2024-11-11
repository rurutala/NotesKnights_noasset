using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIncreaseEffect : MonoBehaviour, IEffect
{
    public int modifierAmount;  // �U���͂̑�����

    public void ApplyEffect(float duration,float modifierAmount)
    {
        this.modifierAmount = (int)modifierAmount;

        StartCoroutine(RemoveEffectAfterDuration(duration));
    }

    private IEnumerator RemoveEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        // �X�N���v�g���̂��폜
        Destroy(this);
    }
}
