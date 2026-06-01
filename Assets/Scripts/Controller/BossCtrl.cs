using System;
using System.Threading.Tasks;
using UnityEngine;
using static BossCtrl;

public class BossCtrl : EnemyCtrl
{
    #region 定義
    /// <summary>
    /// 狀態階段(行為切換基準)
    /// </summary>
    public enum Phase { P0, P1, P2, P3 }
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
    #endregion 專用屬性參數

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
    protected override void Awake()
    {
        base.Awake();//回滿血(初始化)
        GameManager.SetCurrentBoss(this);
    }

    protected override void Update()
    {
        if (_lastPhase != Phase.P0) base.Update();

    }

    public async Task Ready(float time)
    {
        await Task.Delay(TimeSpan.FromSeconds(time));
        _lastPhase = Phase.P1;
        animaCtrl.SetLayerWeight(1, 0f);
    }
    #endregion 生命週期

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
