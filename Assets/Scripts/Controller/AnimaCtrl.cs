using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimaCtrl : MonoBehaviour
{
    #region 基礎元建
    /// <summary>
    /// Animator元件本體(盡量不直接控制)
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// [延遲載入]Animator元件
    /// </summary>
    private Animator animator => _animator ??= GetComponent<Animator>();
    /// <summary>
    /// 角色控制器元件本體
    /// </summary>
    private PlayerCtrl _playerCtrl;
    /// <summary>
    /// [延遲載入]角色控制器元件
    /// </summary>
    private PlayerCtrl playerCtrl => _playerCtrl ??= GetComponentInParent<PlayerCtrl>();
    #endregion 基礎元建

    void Start()
    {
        
    }

    #region 動畫系統基本方法
    /// <summary>
    /// 設置動畫觸發
    /// </summary>
    /// <param name="name">名稱</param>
    public void SetTrigger(int hash) => animator.SetTrigger(hash);

    /// <summary>
    /// 設置動畫布林
    /// </summary>
    /// <param name="name">名稱</param>
    /// <param name="val">值</param>
    public void SetBool(int hash, bool val) => animator.SetBool(hash, val);

    /// <summary>
    /// 設置動畫小數
    /// </summary>
    /// <param name="name">名稱</param>
    /// <param name="val">值</param>
    public void SetFloat(int hash, float val) => animator.SetFloat(hash, val);

    /// <summary>
    /// 設置動畫整數
    /// </summary>
    /// <param name="name">名稱</param>
    /// <param name="val">值</param>
    public void SetInteger(int hash, int val) => animator.SetInteger(hash, val);
    #endregion 動畫系統基本方法

    #region 動畫觸發事件
    public void StartAttack() => playerCtrl?.StartAttack();
    public void OnAttack()
    {

    }
    public void EndAttack() => playerCtrl?.EndAttack();
    public void OpenComboWindow() => playerCtrl?.OpenComboWindow();
    #endregion 動畫觸發事件
}

/// <summary>
/// 動作HASH碼清單
/// </summary>
public static class AniHash
{
    public static readonly int IsMoving = Animator.StringToHash("IsMoving");
    public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    public static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    public static readonly int DashTrigger = Animator.StringToHash("DashTrigger");
    public static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");

    public static readonly int VelocityY = Animator.StringToHash("VelocityY");
    public static readonly int MoveMulti = Animator.StringToHash("MoveMulti");
    public static readonly int Combo = Animator.StringToHash("Combo");
}