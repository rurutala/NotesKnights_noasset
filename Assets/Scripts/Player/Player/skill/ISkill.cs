public interface ISkill
{
    void Activate();  // スキルを発動するメソッド
    bool IsOnCooldown();  // クールダウン中かどうかを確認するメソッド
    float GetCooldownTime();  // クールダウン時間を取得するメソッド
}
