using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeChange : MonoBehaviour
{
    public bool timeson = false;
    public TextMeshProUGUI times_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleTimesChange()
    {
        if(timeson)
        {
            TimeManager.Instance.time_times = 1f;
            timeson = false;
            times_text.text = "1Å~";
            TimeManager.Instance.Time_timesoff();
        }
        else
        {
            TimeManager.Instance.time_times = 2f;
            timeson = true;
            times_text.text = "2Å~";
            TimeManager.Instance.Time_timeson();
        }

        
    }

    public void ScaleStopChange()
    {
        if (timeson)
        {
            GameFlowManager.Instance.Game_Start();
            times_text.text = "é~";
            timeson = false;
        }
        else
        {
            GameFlowManager.Instance.Game_Stop();
            times_text.text = "ìÆ";
            timeson = true;
        }
    }
}
