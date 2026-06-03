using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossCtrl : EnemyCtrl
{
    #region 定義
    /// <summary>
    /// 狀態階段(行為切換基準)
    /// </summary>
    public enum Phase { P0, P1, P2, P3 }
    /// <summary>
    /// 階段組合標籤(Flags複選標籤)
    /// </summary>
    [Flags]
    public enum PhaseFlag
    {
        None = 0,
        P1 = 1 << 0,//1
        P2 = 1 << 1,//10
        P3 = 1 << 2,//100
        All = P1 | P2 | P3//111
    }
    /// <summary>
    /// 當前的狀態階段(用HP百分比計算)
    /// </summary>
    private Phase _currentPhase
    {
        get
        {
            if (PercentHP > _p2Threshold) return Phase.P1;
            if (PercentHP > _p3Threshold) return Phase.P2;
            return Phase.P3;
        }
    }
    private Phase _lastPhase = Phase.P0;
    #endregion 定義

    #region 專用屬性參數
    /// <summary>
    /// 2階段臨界值
    /// </summary>
    [SerializeField]
    private float _p2Threshold = 0.7f;
    /// <summary>
    /// 3階段臨界值
    /// </summary>
    [SerializeField]
    private float _p3Threshold = 0.3f;
    /// <summary>
    /// 切換階段狀態的時間
    /// </summary>
    [SerializeField]
    private float _ptDuration = 3f;
    /// <summary>
    /// 是否處於狀態轉換中
    /// </summary>
    private bool _inPhaseTrans = false;
    /// <summary>
    /// 是否為無敵狀態
    /// </summary>
    private bool _isInvincible = false;


    private float _skillGCD = 3f;
    private float _skillTimer = 0f;
    private bool CanCastSkill => _skillTimer >= _skillGCD;
    #endregion 專用屬性參數

    #region 招式資料庫
    [SerializeField]
    private BossSkillDB[] _skills;
    /// <summary>
    /// 施放中的招式索引號碼
    /// </summary>
    private int _castSkillIndex = -1;
    /// <summary>
    /// 抽取技能時的權重分母
    /// </summary>
    private int totalWeight = 0;
    /// <summary>
    /// 符合施放條件的技能清單
    /// </summary>
    private List<int> skillList = new List<int>();
    /// <summary>
    /// 施放中的技能
    /// </summary>
    private BossSkillDB castingSkill;
    #endregion 招式資料庫

    #region 訂閱事件
    /// <summary>
    /// 階段變換觸發事件
    /// </summary>
    public event Action<Phase> OnPhaseChange;
    /// <summary>
    /// 被討閥觸發事件
    /// </summary>
    public event Action OnDefeated;
    #endregion 訂閱事件

    #region 生命週期

    protected override void Update()
    {
        if (_lastPhase != Phase.P0)
        {
            base.Update();
            //BOSS特技
            SkillCooldown();
        }
    }
    /// <summary>
    /// 預備任務
    /// </summary>
    /// <param name="time">需時</param>
    /// <returns></returns>
    public async Task Ready(float time)
    {
        await Task.Delay(TimeSpan.FromSeconds(time));
        _lastPhase = Phase.P1;//進入第一階段
        animaCtrl.SetLayerWeight(1, 0f);//準備動畫(表演層)關閉
        GameManager.SetCurrentBoss(this);//正式初始化
    }

    private void SkillCooldown()
    {
        if (CanCastSkill)
        {
            Attack();
            _skillTimer = 0;
        }
        else
        {
            _skillTimer += Time.deltaTime;
        }
    }
    #endregion 生命週期

    #region 攻擊行為(招式抽取)
    private void Attack()
    {//不能攻擊或正在切換狀態：不執行Attack
        if (!CanAttack || _inPhaseTrans) return;
        _castSkillIndex = ChooseSkill();
        if (_castSkillIndex > 0)
        {
            castingSkill = _skills[_castSkillIndex];
            ChangeState(State.Attack);
            animaCtrl.SetTrigger(castingSkill.triggerHash);//播放攻擊動畫(前搖)
        }
        
    }
    
    /// <summary>
    /// 抽選技能
    /// </summary>
    /// <returns>技能的序號</returns>
    private int ChooseSkill()
    {
        if (_skills.Length == 0) return -1;

        totalWeight = 0;//權重分母
        skillList.Clear();//清空技能備選清單
        _castSkillIndex = -1;//起點重置

        foreach (BossSkillDB skill in _skills)
        {//遍歷SkillDB
            _castSkillIndex++;//流水號
            if (skill == null || skill.weight <= 0) continue;//該輪跳過
            //if (冷卻未到) continue;
            if (DistanceToTarget < skill.minRange || DistanceToTarget > skill.maxRange) continue;//不再施放範圍內
            if (!IsAllowedPhases(skill.allowedPhases)) continue;//不再施放階段內
            totalWeight += skill.weight;//計算權重分母
            skillList.Add(_castSkillIndex);
        }

        int roll = Random.Range(0, totalWeight);
        foreach (int index in skillList) 
        {
            if (roll < _skills[index].weight) return index;
            else roll -= _skills[index].weight;
        }
        return -1;
    }
    /// <summary>
    /// 階段檢定
    /// </summary>
    /// <param name="flag">階段旗標</param>
    /// <returns>是否包含</returns>
    private bool IsAllowedPhases(PhaseFlag flag)
    {
        
        return _currentPhase switch
        {
            Phase.P1 => (flag & PhaseFlag.P1) != 0,
            Phase.P2 => (flag & PhaseFlag.P2) != 0,
            Phase.P3 => (flag & PhaseFlag.P3) != 0,
            _ => false,
        };
        /*
        switch (_currentPhase)
        {
            case Phase.P1: return (flag & PhaseFlag.P1) != 0;
            case Phase.P2: return (flag & PhaseFlag.P2) != 0;
            case Phase.P3: return (flag & PhaseFlag.P3) != 0;
            default: return false;
        }*/
    }

    public override void OnAttack(Transform point)
    {
        if (!castingSkill) return;
        Instantiate(castingSkill.skillPrefab, point.position, transform.rotation);
    }
    #endregion 攻擊行為(招式抽取)

    #region 傷害階段切換
    public override void TakeDamage(float damage)
    {
        if (_isInvincible) return;
        base.TakeDamage(damage);
        if (!IsDead && !_inPhaseTrans && _currentPhase != _lastPhase) 
        _ = PhaseTranslate(_currentPhase);
    }

    private async Task PhaseTranslate(Phase phase)
    {
        _lastPhase = phase;
        _inPhaseTrans = true;
        _isInvincible = true;
        //切換狀態實際流程
        OnPhaseChange?.Invoke(phase);
        //播放動畫
        animaCtrl.SetTrigger(AniHash.RoarTrigger);
        await Task.Delay(TimeSpan.FromSeconds(_ptDuration));
        _inPhaseTrans = false;
        _isInvincible = false;
        ChangeState(State.Idle);
    }
    #endregion 傷害階段切換
}
