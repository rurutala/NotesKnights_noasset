using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimePause : MonoBehaviour
{
    public static TimePause Instance { get; private set; }

    public bool timeson = false;
    public TextMeshProUGUI times_text;
    public GameObject Pause;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {

        // シングルトンのインスタンスを確立
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 既に存在する場合は新しいオブジェクトを破棄
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleStopChange()
    {
        if (UIManager.Instance.stage_direction) return;
        if (timeson)
        {
            GameFlowManager.Instance.Game_Start();
            times_text.text = "止";
            timeson = false;
            Pause.SetActive(false);
        }
        else
        {
            GameFlowManager.Instance.Game_Stop();
            times_text.text = "動";
            timeson = true;
            Pause.SetActive(true);
        }
    }
}
