using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemovement : MonoBehaviour
{
    public bool canmove = false;
    public string Scenename;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canmove) SceneMove();
    }

    public void SceneMove()
    {
        SceneManager.LoadScene(Scenename);
    }

    public void canScenemove()
    {
        canmove = true;
    }

    public void cannotScenemove()
    {
        canmove = false;
    }

    public void changeScenemove()
    {
        canmove = !canmove;
    }

}
