using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyCtrl : BaseCtrl
{
    #region AI參數
    private Transform _target;
    private Vector2 _aiMoveInput;
    [SerializeField]
    private float _chaseRange = 10f;
    [SerializeField]
    private float _attackRange = 2f;
    private Vector3 _dirToTarget;
    #endregion AI參數

    #region 公用參數
    /// <summary>
    /// 目標對象(玩家)
    /// </summary>
    public Transform Target => _target ??= GameManager.playerCtrl.transform;
    /// <summary>
    /// AI操作輸入
    /// </summary>
    public override Vector2 MoveInput => _aiMoveInput;
    /// <summary>
    /// 對目標的方向向量(忽略高低差)
    /// </summary>
    public Vector3 DirToTarget
    {
        get
        {
            _dirToTarget = Target.position - transform.position;
            _dirToTarget.y = 0;
            return _dirToTarget;
        }
    }
    /// <summary>
    /// 與目標的直線距離(忽略高低差)
    /// </summary>
    public float DistanceToTarget => DirToTarget.magnitude;
    /// <summary>
    /// 是否處於攻擊範圍內
    /// </summary>
    public bool InAttackRange => DistanceToTarget <= _attackRange;
    /// <summary>
    /// 是否處於搜索範圍內
    /// </summary>
    public bool InChaseRange => DistanceToTarget <= _chaseRange;
    #endregion 公用參數

    #region 生命週期(決策)
    // Update is called once per frame
    protected override void Update()
    {
        AIDecision();
        base.Update();
    }

    void AIDecision()
    {
        if (state == State.Attack || state == State.Dash) return;

        if (Target == null)
        {//無目標時保持靜止狀態
            _aiMoveInput = Vector2.zero;
            return;
        }
        //有目標時決策判定
        if (InAttackRange)
        {//立定且攻擊
            _aiMoveInput = Vector2.zero;
            Attack();
        }
        else if (InChaseRange)
        {//模擬搖桿方向輸入
            _aiMoveInput.x = DirToTarget.normalized.x;
            _aiMoveInput.y = DirToTarget.normalized.z;
        }
        else _aiMoveInput = Vector2.zero;
    }
    #endregion 生命週期(決策)

    private void Attack()
    {
        if (IsAttacking && _inComboWindow)
        {
            Combo++;
            _inComboWindow = false;
            AttackHandle();
        }
        else if (!IsAttacking)
        {//完全停止攻擊後：連擊重啟
            Combo = 1;
            AttackHandle();
        }
    }
}
