using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class costspeedtransform : MonoBehaviour, IEffect
{
    public float multicostspeed;

    public void ApplyEffect(float duration, float multicostspeed)
    {
        this.multicostspeed = multicostspeed;

        StartCoroutine(RemoveEffectAfterDuration(duration));
    }

    private IEnumerator RemoveEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        // �X�N���v�g���̂��폜
        Destroy(this);
    }
}
