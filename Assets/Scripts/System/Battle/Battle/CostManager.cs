using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CostManager : MonoBehaviour
{

    public static CostManager Instance { get; private set; }

    public float cost_time = 0;
    [SerializeField]private float cost_time_max;
    public int cost_count = 0;
    [SerializeField] private int cost_count_max;

    public TextMeshProUGUI cost_text;
    public Slider cost_slider;

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
        cost_slider.maxValue = cost_time_max;
        cost_slider.value = 0;
        cost_text_update();
    }

    // Update is called once per frame
    public void CostUpdate()
    {
        cost_time += Time.deltaTime * CalculateTotalCostSpeed();
        if(cost_time > cost_time_max)
        {
            cost_time = 0;
            if(cost_count + 1 != cost_count_max) cost_count++;
            cost_text_update();
        }
        cost_slider.value = cost_time;
    }

    public void ConsumeCost(int consume)
    {
        
        cost_count -= consume;

        if(cost_count < 0)
        {
            cost_count = 0;
        }

        cost_text_update();
    }
    public void AddCost(int add)
    {
        cost_count += add;
        if(cost_count > cost_count_max)
        {
            cost_count = cost_count_max;
        }

        cost_text_update();
    }

    public bool CanCost(int cost)
    {
        if (cost_count - cost < 0) return false;
        return true;
    }
    public void cost_text_update()
    {
        cost_text.text = cost_count.ToString();
    }

    // 結果を計算して返す関数
    public float CalculateTotalCostSpeed()
    {
        // 自身にアタッチされているcostspeedtransformスクリプトをすべて取得
        costspeedtransform[] costSpeedTransforms = GetComponents<costspeedtransform>();

        // 初期値を1に設定（掛け算のため）
        float totalCostSpeed = 1f;

        // 取得したスクリプトのmulticostspeedを順に掛け算
        foreach (costspeedtransform costSpeed in costSpeedTransforms)
        {
            totalCostSpeed *= costSpeed.multicostspeed;
        }

        // 計算結果を返す
        return totalCostSpeed;
    }
}
