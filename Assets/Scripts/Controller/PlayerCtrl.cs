using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

//預設必須的元件
[RequireComponent(typeof(CharacterController))]
public class PlayerCtrl : MonoBehaviour
{
    #region 基礎元建
    /// <summary>
    /// CharacterController元件本體(盡量不直接控制)
    /// </summary>
    private CharacterController _charCtrl;
    /// <summary>
    /// [延遲載入]CharacterController元件
    /// </summary>
    private CharacterController charCtrl => _charCtrl ??= GetComponent<CharacterController>();
    /// <summary>
    /// AnimaCtrl元件本體
    /// </summary>
    private AnimaCtrl _animaCtrl;
    /// <summary>
    /// [延遲載入]AnimaCtrl元件
    /// </summary>
    private AnimaCtrl animaCtrl => _animaCtrl ??= GetComponentInChildren<AnimaCtrl>();
    #endregion 基礎元建

    #region 狀態機
    /// <summary>
    /// 狀態機定義
    /// </summary>
    public enum State { Idle, Move, Jump, Dash, Attack }
    /// <summary>
    /// 角色當前狀態
    /// </summary>
    public State state = State.Idle;
    /// <summary>
    /// 切換狀態
    /// </summary>
    /// <param name="state">新狀態</param>
    private void ChangeState(State state)
    {
        if (this.state == state) return;
        this.state = state;
    }
    private void StateLogic()
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
    private Controls _controls;
    private Vector3 _facingVector;
    private Vector3 _velocity;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _jumpHeight = 3f;
    private float _jumpPower = 1f;
    [SerializeField]
    private int _airJumpCountMax = 1;
    private int _airJumpCount;
    [SerializeField]
    private float _dashSpeed = 8f;
    private float _dashDuration = 0.2f;
    
    private int _combo;
    private bool _inComboWindow;
    #endregion 基本參數

    #region 公用參數
    /// <summary>
    /// 產生一組預設好的控制檔
    /// </summary>
    public Controls InputCtrl => _controls ??= new Controls();
    /// <summary>
    /// 從輸入取得的方向向量
    /// </summary>
    public Vector2 MoveInput => InputCtrl.Play.Move.ReadValue<Vector2>();
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
    /// 依據方向向量輸入判定是否在移動中
    /// </summary>
    public bool IsMoving => MoveInput != Vector2.zero;
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
    /// 是否可以執行空中跳躍
    /// </summary>
    public bool CanAirJump => _airJumpCount > 0;
    /// <summary>
    /// 用於位移的動能
    /// </summary>
    public Vector3 Velocity => _velocity * Time.deltaTime;
    public float VelocityY => _velocity.y;

    public int Combo
    {
        get
        {
            return _combo;
        }
        set
        {
            _combo = value;
            Debug.Log(_combo);
            if (_combo > 2) _combo = 1;
        }
    }
    #endregion 公用參數

    #region 生命週期
    private void OnEnable()
    {
        InputCtrl.Play.Enable();
        //操作行為事件訂閱
        InputCtrl.Play.Jump.performed += Jump;
        InputCtrl.Play.Attack.performed += Attack;
        InputCtrl.Play.Dash.performed += Dash;
    }


    private void OnDisable()
    {
        InputCtrl.Play.Disable();
        //操作行為事件訂閱取消
        InputCtrl.Play.Jump.performed -= Jump;
        InputCtrl.Play.Attack.performed -= Attack;
        InputCtrl.Play.Dash.performed -= Dash;
    }

    /// <summary>
    /// 狀態刷新
    /// </summary>
    void Update()
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
        animaCtrl.SetFloat(AniHash.MoveMulti, MoveMulti);
        animaCtrl.SetFloat(AniHash.VelocityY, VelocityY);
        animaCtrl.SetInteger(AniHash.Combo, Combo);
    }
    #endregion 生命週期

    #region 角色物理控制
    /// <summary>
    /// 動態套用
    /// </summary>
    void Movement()
    {
        Gravity();//重力
        charCtrl.Move(Velocity);
    }
    /// <summary>
    /// 重力
    /// </summary>
    void Gravity()
    {
        if (IsGrounded)
        {
            _velocity.y = -1f;
            _airJumpCount = _airJumpCountMax;
            _jumpPower = 1f;
        }
        else if (state != State.Dash)
        {
            _velocity.y -= G * Time.deltaTime;
        }
    }

    /// <summary>
    /// 轉向事件
    /// </summary>
    void Rota()
    {//轉向
        if (FacingVector != Vector3.zero)
        charCtrl.transform.rotation = Quaternion.LookRotation(FacingVector);
    }
    #endregion 角色物理控制

    #region 跳躍功能
    /// <summary>
    /// 跳躍事件
    /// </summary>
    /// <param name="context">接收輸入</param>
    void Jump(InputAction.CallbackContext context)
    {
        if (state == State.Attack || state == State.Dash) return;

        if (IsGrounded) 
        {
            JumpHandle();
        }
        else if (CanAirJump) 
        {
            _airJumpCount--;
            _jumpPower = 0.5f;
            JumpHandle();
        }
    }

    void JumpHandle()
    {
        ChangeState(State.Jump);
        _velocity.y = Mathf.Sqrt(2 * G * H);
        animaCtrl.SetTrigger(AniHash.JumpTrigger);
    }
    #endregion 跳躍功能

    #region 攻擊功能
    private void Attack(InputAction.CallbackContext context)
    {
        if (state == State.Dash) return;
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

    public void AttackHandle()
    {
        ChangeState(State.Attack);
        animaCtrl.SetTrigger(AniHash.AttackTrigger);
    }

    public void StartAttack()
    {
        _inComboWindow = false;
    }

    public void EndAttack()
    {
        _inComboWindow = false;
        if (state == State.Attack)
        {
            ChangeState(IsGrounded ? State.Idle : State.Jump);
        }
    }

    public void OpenComboWindow()
    {
        _inComboWindow = true;
    }
    #endregion 攻擊功能

    #region 衝刺功能
    private void Dash(InputAction.CallbackContext context)
    {
        if (state == State.Attack || state == State.Dash) return;
        ChangeState(State.Dash);
        animaCtrl.SetTrigger(AniHash.DashTrigger);

        _ = DashHandle();
    }

    private async Task DashHandle()
    {
        charCtrl.transform.rotation = Quaternion.LookRotation(transform.forward);
        _velocity = transform.forward * _dashSpeed;
        _velocity.y = 0;
        //推進
        await Task.Delay(TimeSpan.FromSeconds(_dashDuration));

        if (state == State.Dash)
        {
            _velocity = Vector3.zero;
            ChangeState(IsGrounded ? State.Idle : State.Jump);
        }
    }
    #endregion 衝刺功能
}
