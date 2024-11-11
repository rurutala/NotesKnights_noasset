public interface IEffect
{
    // 効果を適用するメソッド、加算方法（固定かパーセンテージ）と効果時間を指定
    void ApplyEffect(float duration, float modifierAmount);
}
