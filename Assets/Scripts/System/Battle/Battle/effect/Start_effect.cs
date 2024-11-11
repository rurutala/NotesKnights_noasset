using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_effect : MonoBehaviour
{
    public GameObject blockUI;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.stage_direction = true;
        GameFlowManager.Instance.Game_Stop();
        blockUI.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void call_Game_Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.bgmList[0]);

        UIManager.Instance.stage_direction = false;
        GameFlowManager.Instance.Game_Start();
        blockUI.SetActive(false);
        Destroy(this.gameObject);
    }
}
