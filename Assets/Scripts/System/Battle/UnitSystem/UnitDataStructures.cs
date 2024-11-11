using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public int unitID;       // ���j�b�g�ŗL��ID
    public string unitName;  // ���j�b�g�̖��O
    public Sprite unitIcon;  // UI�ɕ\�����邽�߂̃A�C�R��
}

[System.Serializable]
public class PlayerDeck
{
    public List<UnitData> units; // �v���C���[���I���������j�b�g�̃��X�g
}
