using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_audio : MonoBehaviour
{
    public AudioClip attackse;
    public AudioClip skillse;
    public AudioClip deathse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attack_se()
    {
        if(attackse != null)
        AudioManager.Instance.PlaySE(attackse);
    }

    public void skill_se()
    {
        if (skillse != null)
            AudioManager.Instance.PlaySE(skillse);
    }

    public void death_se()
    {
        if (deathse != null)
            AudioManager.Instance.PlaySE(deathse);
    }
}
