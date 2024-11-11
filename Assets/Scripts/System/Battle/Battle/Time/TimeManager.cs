using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public float timeScale = 1f;
    public float time_times = 1f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ���łɃC���X�^���X�����݂���ꍇ�A���̃I�u�W�F�N�g��j��
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimeScaleChange(float time)
    {
        if (UIManager.Instance.stage_direction) return;
        timeScale = time * time_times;
        Time.timeScale = timeScale;
    }

    public void Time_timeson()
    {
        if (UIManager.Instance.stage_direction) return;
        timeScale = timeScale * time_times;
        Time.timeScale = timeScale;
    }

    public void Time_timesoff()
    {
        if (UIManager.Instance.stage_direction) return;
        timeScale = timeScale * time_times * 0.5f;
        Time.timeScale = timeScale;
    }


}
