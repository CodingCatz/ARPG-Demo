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

    #region 基本參數
    private Controls _controls;
    private Vector3 _facingVector;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _jumpHeight = 3f;
    private Vector3 _velocity;
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
    public float MoveMulti => MoveInput.magnitude;
    public float MoveSpeed => MoveInput.magnitude * _moveSpeed;
    public float G => Mathf.Abs(Physics.gravity.y);
    public float H => _jumpHeight;

    public Vector3 Velocity => _velocity * Time.deltaTime;
    #endregion 公用參數

    #region 生命週期
    private void OnEnable()
    {
        InputCtrl.Play.Enable();
        //操作行為事件訂閱
        InputCtrl.Play.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        InputCtrl.Play.Disable();
        //操作行為事件訂閱取消
        InputCtrl.Play.Jump.performed -= Jump;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// 狀態刷新
    /// </summary>
    void Update()
    {
        AnimaUpdate();
        Rota();
        Movement();
    }
    /// <summary>
    /// 動畫更新
    /// </summary>
    void AnimaUpdate()
    {
        animaCtrl.SetBool("IsMoving", IsMoving);
        animaCtrl.SetFloat("MoveMulti", MoveMulti);
        
    }
    #endregion 生命週期
    /// <summary>
    /// 動態套用
    /// </summary>
    void Movement()
    {
        _velocity.z = transform.forward.z * MoveSpeed;
        _velocity.x = transform.forward.x * MoveSpeed;
        //重力
        if (charCtrl.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1f; 
        }
        else
        {
            _velocity.y -= G * Time.deltaTime;
        }

        charCtrl.Move(Velocity);
    }

    /// <summary>
    /// 轉向事件
    /// </summary>
    void Rota()
    {
        if (!IsMoving) return;
        //轉向
        charCtrl.transform.rotation = Quaternion.LookRotation(FacingVector);
    }

    /// <summary>
    /// 跳躍事件
    /// </summary>
    /// <param name="context">接收輸入</param>
    void Jump(InputAction.CallbackContext context)
    {
        //向上
        _velocity.y = Mathf.Sqrt(2 * G * H);
    }
}
