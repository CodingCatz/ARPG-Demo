using System;
using UnityEngine;

/// <summary>
/// 角色控制器抽象層
/// </summary>
[RequireComponent(typeof(CharacterController))]
public abstract class BaseCtrl : MonoBehaviour
{
    #region 基礎元建
    /// <summary>
    /// CharacterController元件本體(盡量不直接控制)
    /// </summary>
    private CharacterController _charCtrl;
    /// <summary>
    /// [延遲載入]CharacterController元件
    /// </summary>
    protected CharacterController charCtrl => _charCtrl ??= GetComponent<CharacterController>();
    /// <summary>
    /// AnimaCtrl元件本體
    /// </summary>
    private AnimaCtrl _animaCtrl;
    /// <summary>
    /// [延遲載入]AnimaCtrl元件
    /// </summary>
    protected AnimaCtrl animaCtrl => _animaCtrl ??= GetComponentInChildren<AnimaCtrl>();
    #endregion 基礎元建

    #region 狀態機
    /// <summary>
    /// 狀態機定義
    /// </summary>
    public enum State { Idle, Move, Jump, Dash, Attack, Hit, Dead }
    /// <summary>
    /// 角色當前狀態
    /// </summary>
    protected State state = State.Idle;
    /// <summary>
    /// 切換狀態
    /// </summary>
    /// <param name="state">新狀態</param>
    protected void ChangeState(State state)
    {
        if (this.state == state) return;
        this.state = state;
    }
    protected void StateLogic()
    {
        switch (state)
        {
            case State.Idle:
                if (IsMoving) ChangeState(State.Move);
                if (!IsGrounded) ChangeState(State.Jump);//下墜(無起跳過程)
                break;

            case State.Move:
                Rota();
                _velocity.z = transform.forward.z * MoveSpeed;
                _velocity.x = transform.forward.x * MoveSpeed;
                if (!IsMoving) ChangeState(State.Idle);
                if (!IsGrounded) ChangeState(State.Jump);//下墜(無起跳過程)
                break;

            case State.Jump:
                Rota();
                _velocity.z = transform.forward.z * MoveSpeed;
                _velocity.x = transform.forward.x * MoveSpeed;
                if (IsGrounded) ChangeState(IsMoving ? State.Move : State.Idle);
                break;

            case State.Dash:
                //未實作
                break;

            case State.Attack:
                _velocity.z = 0;
                _velocity.x = 0;
                break;
        }
    }
    #endregion 狀態機

    #region 基本參數
    protected Vector3 _facingVector;
    protected Vector3 _velocity;
    [SerializeField]
    protected float _moveSpeed = 5f;
    [SerializeField]
    protected float _jumpHeight = 3f;
    protected float _jumpPower = 1f;

    protected int _combo;
    protected bool _inComboWindow;
    [SerializeField]
    protected GameObject[] _skillPrefabs;
    #endregion 基本參數

    #region 角色屬性參數
    /// <summary>
    /// 最大生命值
    /// </summary>
    [SerializeField]
    protected float _maxHP = 100f;
    /// <summary>
    /// 當前的生命值
    /// </summary>
    protected float _HP;
    protected event Action<float, float> OnHPChanged;
    //===== 屬性公用參數 =====
    public float CurrentHP => _HP;
    public float MaxHP => _maxHP;
    public bool IsDead => state == State.Dead;
    #endregion 角色屬性參數

    #region 抽象公用屬性參數
    /// <summary>
    /// 取得操作的方向向量
    /// </summary>
    public abstract Vector2 MoveInput { get; }
    /// <summary>
    /// 面向的方向向量
    /// </summary>
    public Vector3 FacingVector
    {
        get
        {
            _facingVector.x = MoveInput.x;
            _facingVector.z = MoveInput.y;
            return _facingVector;
        }
    }
    /// <summary>
    /// 角度補償
    /// </summary>
    public virtual Quaternion AngComp => Quaternion.identity;
    /// <summary>
    /// 依據方向向量輸入判定是否在移動中
    /// </summary>
    public bool IsMoving => MoveInput != Vector2.zero;
    /// <summary>
    /// 是否正在執行攻擊動作
    /// </summary>
    public bool IsAttacking => state == State.Attack;
    /// <summary>
    /// 移動倍率(標準化 0~1)
    /// </summary>
    public float MoveMulti => MoveInput.magnitude;
    /// <summary>
    /// 當前移動可達速度
    /// </summary>
    public float MoveSpeed => MoveInput.magnitude * _moveSpeed;
    /// <summary>
    /// 重力值
    /// </summary>
    public float G => Mathf.Abs(Physics.gravity.y);
    /// <summary>
    /// 當前跳躍可達高度
    /// </summary>
    public float H => _jumpHeight * _jumpPower;
    /// <summary>
    /// 是否處於觸地狀態
    /// </summary>
    public bool IsGrounded => charCtrl.isGrounded && _velocity.y < 0;
    /// <summary>
    /// 用於位移的動能
    /// </summary>
    public Vector3 Velocity => _velocity * Time.deltaTime;
    public float VelocityY => _velocity.y;

    public int Combo
    {
        get => _combo;
        set
        {
            _combo = value;
            if (_combo > 2) _combo = 1;
        }
    }
    #endregion 抽象公用屬性參數

    #region 生命週期
    protected virtual void Awake()
    {
        _HP = _maxHP;//登場回滿血(初始化)
    }
    /// <summary>
    /// 狀態刷新
    /// </summary>
    protected virtual void Update()
    {
        StateLogic();
        AnimaUpdate();
        Movement();
    }
    /// <summary>
    /// 動畫更新
    /// </summary>
    void AnimaUpdate()
    {
        animaCtrl.SetBool(AniHash.IsMoving, IsMoving);
        animaCtrl.SetBool(AniHash.IsGrounded, IsGrounded);
        animaCtrl.SetBool(AniHash.IsAttacking, IsAttacking);
        animaCtrl.SetBool(AniHash.IsDead, IsDead);
        animaCtrl.SetFloat(AniHash.MoveMulti, MoveMulti);
        animaCtrl.SetFloat(AniHash.VelocityY, VelocityY);
        animaCtrl.SetInteger(AniHash.Combo, Combo);
    }
    #endregion 生命週期

    #region 物理控制
    /// <summary>
    /// 動態套用
    /// </summary>
    protected void Movement()
    {
        Gravity();//重力
        if (charCtrl.enabled) charCtrl.Move(Velocity);
    }
    /// <summary>
    /// 重力
    /// </summary>
    protected virtual void Gravity()
    {
        if (IsGrounded)
        {
            if (IsDead)
            {
                _velocity = Vector3.zero;
                charCtrl.enabled = false;
            }
            else
            {
                _velocity.y = -1f;
                _jumpPower = 1f;
            }
        }
        else if (state != State.Dash)
        {
            _velocity.y -= G * Time.deltaTime;
        }
    }
    /// <summary>
    /// 轉向事件
    /// </summary>
    protected void Rota()
    {//轉向
        if (FacingVector != Vector3.zero)
            charCtrl.transform.rotation = Quaternion.LookRotation(FacingVector) * AngComp;
    }
    #endregion 物理控制

    #region 基礎動作與戰鬥
    protected void JumpHandle()
    {
        ChangeState(State.Jump);
        _velocity.y = Mathf.Sqrt(2 * G * H);
        animaCtrl.SetTrigger(AniHash.JumpTrigger);
    }

    protected void AttackHandle()
    {
        ChangeState(State.Attack);
        animaCtrl.SetTrigger(AniHash.AttackTrigger);
    }

    #endregion 基礎動作與戰鬥

    #region 受擊與傷害邏輯
    protected virtual void SetOnHPChangedEvent(Action<float, float> action)
    {
        OnHPChanged = action;
        OnHPChanged?.Invoke(CurrentHP, MaxHP);
    }
    /// <summary>
    /// 共通傷害執行接口
    /// </summary>
    /// <param name="damage">傷害值</param>
    public virtual void TakeDamage(float damage)
    {
        if (IsDead) return;//避免鞭屍
        _HP -= damage;
        if (damage > 0)
        {
            HitHandle();
            //有UI事件訂閱：執行數值傳遞
            OnHPChanged?.Invoke(CurrentHP, MaxHP);
        }
        if (_HP <= 0) Die();
    }
    /// <summary>
    /// 觸發受傷狀態與動畫
    /// </summary>
    protected virtual void HitHandle()
    {
        ChangeState(State.Hit);
        _velocity.x = 0;
        _velocity.z = 0;
        animaCtrl.SetTrigger(AniHash.HitTrigger);
    }
    /// <summary>
    /// 觸發死亡狀態與動畫
    /// </summary>
    protected virtual void Die()
    {
        _HP = 0;
        ChangeState(State.Dead);
        //下墜中的物理落地後執行
        //_velocity = Vector3.zero;
        //charCtrl.enabled = false;
        animaCtrl.SetTrigger(AniHash.DeadTrigger);
    }

    public void EndHit()
    {
        if (state == State.Hit)
            ChangeState(IsGrounded ? State.Idle : State.Jump);
    }
    #endregion 受擊與傷害邏輯

    #region 動畫控制取用
    public void StartAttack() => _inComboWindow = false;
    public void OpenComboWindow() => _inComboWindow = true;
    public void EndAttack()
    {
        _inComboWindow = false;
        if (state == State.Attack)
        {
            ChangeState(IsGrounded ? State.Idle : State.Jump);
        }
    }

    public void OnAttack(Transform point)
    {
        if (_skillPrefabs == null || _skillPrefabs.Length == 0) return;
        Instantiate(_skillPrefabs[0], point.position, transform.rotation);
    }
    #endregion 動畫控制取用
}
