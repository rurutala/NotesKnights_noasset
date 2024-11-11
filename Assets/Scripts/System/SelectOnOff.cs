using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOnOff : MonoBehaviour
{

    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onObject()
    {
        gameObject.SetActive(true);
    }

    public void offObject()
    {
        gameObject.SetActive(false); 
    }
}
