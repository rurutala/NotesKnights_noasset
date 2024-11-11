using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3kill : MonoBehaviour, ISkill
{
    public float cooldownTime = 10f;  // クールダウン時間
    private bool isOnCooldown = false;

    public int cure_cost = 2;

    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // スキルの効果をここに実装する
            CostUp();

            StartCoroutine(CooldownRoutine());
        }
        else
        {
            Debug.Log("Skill is on cooldown.");
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCooldownTime()
    {
        return cooldownTime;
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
        Debug.Log("Skill is ready again.");
    }


    private void CostUp()
    {
        CostManager.Instance.AddCost(cure_cost);
    }
}
