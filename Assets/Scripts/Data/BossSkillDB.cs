using UnityEngine;

[CreateAssetMenu(fileName = "BossSkillDB", menuName = "DataBase/BossSkillDB")]
public class BossSkillDB : ScriptableObject
{
    #region 識別訊息
    /// <summary>
    /// 招式名稱(除錯訊息)
    /// </summary>
    public string id;
    /// <summary>
    /// 觸發動畫的Trigger名稱
    /// </summary>
    public string aniTriggerName;
    #endregion 識別訊息

    #region 投射物設定
    /// <summary>
    /// 技能本體預製物件
    /// </summary>
    public SkillCtrl skillPrefab;
    /// <summary>
    /// 最小觸發範圍
    /// </summary>
    public float minRange = 0f;
    /// <summary>
    /// 最大觸發範圍
    /// </summary>
    public float maxRange = 5f;
    /// <summary>
    /// 冷卻時間
    /// </summary>
    public float cooldown = 10f;
    /// <summary>
    /// 抽選權重
    /// </summary>
    public int weight = 1;
    /// <summary>
    /// 複選可使用的階段(過濾)
    /// </summary>
    public BossCtrl.PhaseFlag allowedPhases = BossCtrl.PhaseFlag.All;
    #endregion 投射物設定
}
