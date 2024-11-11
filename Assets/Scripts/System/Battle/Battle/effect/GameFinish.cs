using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinish : MonoBehaviour
{
    public static GameFinish Instance { get; private set; }

    public int addmoney;
    public int addunitmoney;

    public GameObject endpanel;
    public GameObject gameover_text;
    public GameObject gameclear_text;
    public GameObject blockUI;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        endpanel.SetActive(true);
        gameover_text.SetActive(true);
        blockUI.SetActive(true);
        UIManager.Instance.stage_direction = true;
    }
    public void GameClear()
    {
        endpanel.SetActive(true);
        gameclear_text.SetActive(true);
        blockUI.SetActive(true);
        UIManager.Instance.stage_direction = true;
    }

    public void moveclear()
    {
        DataManager.Instance.event_on = true;
        DataManager.Instance.clearcount += 1;
        DataManager.Instance.currentstage = DataManager.Instance.nextcurrentstage;
        DataManager.Instance.money += cal_money(addmoney);
        DataManager.Instance.moneyunit += cal_unitmoney(addunitmoney);
        SceneManager.LoadScene("Game_select");
    }

    public void movelose()
    {
        DataManager.Instance.event_on = true;
        DataManager.Instance.money += addmoney;
        DataManager.Instance.moneyunit += addunitmoney;
        DataManager.Instance.remaincount += 1;
        SceneManager.LoadScene("Game_select");
    }

    public int cal_money(int money)
    {
        money = (int)(GameFlowManager.Instance.TolerableEnemiesCount * 0.5 * money);
        return money;
    }

    public int cal_unitmoney(int unitmoney)
    {
        unitmoney = (int)(GameFlowManager.Instance.TolerableEnemiesCount * 0.5 * unitmoney);
        return unitmoney;
    }
}
