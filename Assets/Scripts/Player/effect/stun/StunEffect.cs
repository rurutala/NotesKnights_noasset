using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : MonoBehaviour, IEffect
{
    public bool isstun;
    
    public void ApplyEffect(float duration, float modifierAmount)
    {
        isstun = true;

        StartCoroutine(RemoveEffectAfterDuration(duration));
    }

    private IEnumerator RemoveEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        // スクリプト自体を削除
        Destroy(this);
    }
}
