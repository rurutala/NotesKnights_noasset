using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockbuy : MonoBehaviour
{
    public GameObject lockpanel;
    public int currentcost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatecost(int cost)
    {
        currentcost = cost;
        if(currentcost <= DataManager.Instance.moneyunit)
        {
            lockpanel.SetActive(false);
        }
        else
        {
            lockpanel.SetActive(true);
        }
    }

    public void dec_moneyunit()
    {
        DataManager.Instance.moneyunit -= currentcost;
    }
}
