
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyCtrl : BaseCtrl
{
    #region AI參數
    private Transform _target;
    private Vector2 _aiMoveInput;
    [SerializeField]
    private float _patrolRange = 3f;
    private bool _patrolling;
    private Vector3 _patrolPos;
    [SerializeField]
    private float _chaseRange = 10f;
    [SerializeField]
    private float _attackRange = 2f;

    private Vector3 _dirToTarget;
    private Vector3 _spawnPos;
    [SerializeField]
    private float _attackCD = 2f;
    private float _attackTimer;
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
    /// <summary>
    /// 巡邏方位
    /// </summary>
    public Vector3 DirToPatrol
    {
        get
        {
            _patrolPos.y = 0;
            return (_patrolPos - transform.position).normalized;
        }
    }
    /// <summary>
    /// 是否處於搜索範圍內
    /// </summary>
    public bool IsPatrolDone => Vector3.Distance(transform.position, _patrolPos) < 0.1f;
    /// <summary>
    /// 是否冷卻完成：可進行攻擊
    /// </summary>
    public bool CanAttack => _attackTimer <= 0;
    #endregion 公用參數

    #region 生命週期(決策)
    private void Start()
    {//紀錄出生座標 & 巡邏原點
        _spawnPos = transform.position;
        _patrolPos = transform.position;
    }
    protected override void Update()
    {
        AttackCoolDown();
        AIDecision();
        base.Update();
    }
    /// <summary>
    /// 攻擊冷卻
    /// </summary>
    void AttackCoolDown()
    {
        if (!CanAttack) _attackTimer -= Time.deltaTime;
    }
    /// <summary>
    /// AI決策邏輯
    /// </summary>
    void AIDecision()
    {
        if (IsDead || state == State.Hit || state == State.Attack || state == State.Dash) return;

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
        else Patrol();
    }
    /// <summary>
    /// 目標不在搜索範圍：巡邏
    /// </summary>
    private void Patrol()
    {
        if (IsPatrolDone)
        {//隨機產生巡邏點
            _patrolPos = _spawnPos + Random.insideUnitSphere * _patrolRange;
        }
        else
        {
            //往目標方向推進
            _aiMoveInput.x = DirToPatrol.x * 0.3f;
            _aiMoveInput.y = DirToPatrol.z * 0.3f;
        }
    }
    #endregion 生命週期(決策)

    #region 戰鬥行動
    private void Attack()
    {
        if (CanAttack)
        {//冷卻完成發動攻擊
            Combo++;
            _attackTimer = _attackCD;
            AttackHandle();
        }
        else
        {//死盯著目標對象
            charCtrl.transform.rotation = Quaternion.LookRotation(DirToTarget);
        }
    }

    #endregion 戰鬥行動
}
