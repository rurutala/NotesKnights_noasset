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

        // �V���O���g���̃C���X�^���X���m��
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͐V�����I�u�W�F�N�g��j��
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
            times_text.text = "�~";
            timeson = false;
            Pause.SetActive(false);
        }
        else
        {
            GameFlowManager.Instance.Game_Stop();
            times_text.text = "��";
            timeson = true;
            Pause.SetActive(true);
        }
    }
}
