using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    public Animator animator;
    [SerializeField]private EnemyBattle enemybattle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeImageforDirection()
    {
        // Œ»İ‚Ì‰ñ“]Šp“x‚ğæ“¾
        Vector3 currentRotation = transform.eulerAngles;

        // y²‚Ì‰ñ“]‚ğ”½“]‚³‚¹‚éi180“x‰ñ“]j
        currentRotation.y += 180;

        // ‰ñ“]‚ğ”½‰f
        transform.eulerAngles = currentRotation;
    }

        public void isWaiting()
    {
        animator.SetBool("isWait", true);
        animator.SetBool("isMove", false);
    }

    public void isMoving()
    {
        animator.SetBool("isWait", false);
        animator.SetBool("isMove", true);
    }

    public void setAttack()
    {
        animator.SetTrigger("isAttack");


    }
    public void setDeath()
    {
        animator.SetTrigger("isDeath");
        //enemybattle.Die();
        enemybattle.Die();
    }

    public void callDie()
    {
        enemybattle.Die();
    }
}
