using System;
using UnityEngine;

public class BossCtrl : EnemyCtrl
{

    #region 訂閱事件
    public event Action<float, float> onHPChanged;
    #endregion 訂閱事件

    #region 生命週期
    protected override void Awake()
    {
        base.Awake();//回滿血(初始化)
        GameManager.SetCurrentBoss(this);
    }
    #endregion 生命週期
}
