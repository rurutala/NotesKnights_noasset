using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startBattle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle()
    {
        UnitSelectManager.Instance.UnitDataSend();
        SceneManager.LoadScene("Game" + StageSelectManger.Instance.nextstageid);
    }
}
