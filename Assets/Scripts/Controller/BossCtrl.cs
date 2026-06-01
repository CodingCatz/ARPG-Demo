using System;
using UnityEngine;

public class BossCtrl : EnemyCtrl
{
    #region 定義
    /// <summary>
    /// 狀態階段(行為切換基準)
    /// </summary>
    public enum Phase { P1, P2, P3 }
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
    #endregion 定義

    #region 專用屬性參數
    [SerializeField]
    private float _p2Threshold = 0.7f;
    [SerializeField]
    private float _p3Threshold = 0.3f;
    #endregion 專用屬性參數

    #region 訂閱事件

    #endregion 訂閱事件

    #region 生命週期
    protected override void Awake()
    {
        base.Awake();//回滿血(初始化)
        GameManager.SetCurrentBoss(this);
    }
    #endregion 生命週期
}
