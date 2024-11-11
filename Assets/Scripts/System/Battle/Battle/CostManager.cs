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

    // ���ʂ��v�Z���ĕԂ��֐�
    public float CalculateTotalCostSpeed()
    {
        // ���g�ɃA�^�b�`����Ă���costspeedtransform�X�N���v�g�����ׂĎ擾
        costspeedtransform[] costSpeedTransforms = GetComponents<costspeedtransform>();

        // �����l��1�ɐݒ�i�|���Z�̂��߁j
        float totalCostSpeed = 1f;

        // �擾�����X�N���v�g��multicostspeed�����Ɋ|���Z
        foreach (costspeedtransform costSpeed in costSpeedTransforms)
        {
            totalCostSpeed *= costSpeed.multicostspeed;
        }

        // �v�Z���ʂ�Ԃ�
        return totalCostSpeed;
    }
}
