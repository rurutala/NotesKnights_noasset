using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option_openclose : MonoBehaviour
{
    public bool Option_open;
    public GameObject OptionPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.stage_direction) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Optionopenclose();
        }
    }

    public void Optionopenclose()
    {
        if (Option_open)
        {
            Option_open = false;
            OptionPanel.gameObject.SetActive(false);
            GameFlowManager.Instance.Game_Start();
            if (TimePause.Instance.timeson)
            {
                TimePause.Instance.ScaleStopChange();
            }
        }
        else
        {
            Option_open = true;
            OptionPanel.gameObject.SetActive(true);
            GameFlowManager.Instance.Game_Stop();
        }
    }
}
