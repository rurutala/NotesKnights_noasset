public interface ISkill
{
    void Activate();  // �X�L���𔭓����郁�\�b�h
    bool IsOnCooldown();  // �N�[���_�E�������ǂ������m�F���郁�\�b�h
    float GetCooldownTime();  // �N�[���_�E�����Ԃ��擾���郁�\�b�h
}
