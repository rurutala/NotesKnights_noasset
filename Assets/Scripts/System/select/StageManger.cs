using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Stage
{
    Modeselect,
    Stageselect,
    Event,
    Party
}
public class StageManger : MonoBehaviour
{
    public static StageManger Instance { get; private set; }

    public Stage current_stage;

    public GameObject _mode;
    public GameObject _stageselect;
    public GameObject _event;
    public GameObject _party;

    public int current_stageid;

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

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        DataManager.Instance.playerDeck.units.Clear();
        if(DataManager.Instance.remaincount == 4)
        {
            MoveGameover();
        }
        if(DataManager.Instance.clearcount == 3)
        {
            MoveGameclear();
        }

        if (!DataManager.Instance.stageselected)
        {
            current_stage = Stage.Modeselect;
            Mode_on();
        }
        else if (DataManager.Instance.event_on)
        {
            AudioManager.Instance.PlayBGM(null);
            AudioManager.Instance.PlayBGM(AudioManager.Instance.bgmList[1]);
            current_stage =Stage.Event;
            Event_on();
            DataManager.Instance.event_on = false;
        }
        else
        {
            AudioManager.Instance.PlayBGM(AudioManager.Instance.bgmList[1]);
            current_stage = Stage.Stageselect;
            Stage_select_on();
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (current_stage)
        {
            case Stage.Modeselect:
                
                break;
             case Stage.Stageselect:
                StageSelectManger.Instance.selectmanagerUpdate();
                break;
            case Stage.Event:

                break;
            case Stage.Party:

                break;
        }
    }

    public void Mode_on()
    {
        _mode.SetActive(true);
        current_stage = Stage.Modeselect;
    }
    public void Mode_off()
    {
        DataManager.Instance.stageselected = true;
        _mode.SetActive(false); 
    }

    public void Stage_select_on()
    {
        _stageselect.SetActive(true);
        current_stage = Stage.Stageselect;
    }

    public void Stage_select_off()
    {
        _stageselect.SetActive(false);
    }

    public void Event_on()
    {
        _event.SetActive(true);
        current_stage = Stage.Event;
    }

    public void Event_off()
    {
        _event.SetActive(false);
    }

    public void Party_on()
    {
        _party.SetActive(true);
        current_stage = Stage.Party;
    }

    public void Party_off()
    {
        _party.SetActive(false);
    }

    public void MoveGameover()
    {
        SceneManager.LoadScene("gameover");
    }

    public void MoveGameclear()
    {
        SceneManager.LoadScene("end");
    }
}
