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
    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    /// <summary>
    /// 設置動畫布林
    /// </summary>
    /// <param name="name">名稱</param>
    /// <param name="val">值</param>
    public void SetBool(string name, bool val)
    {
        animator.SetBool(name, val);
    }

    /// <summary>
    /// 設置動畫小數
    /// </summary>
    /// <param name="name">名稱</param>
    /// <param name="val">值</param>
    public void SetFloat(string name, float val)
    {
        animator.SetFloat(name, val);
    }

    /// <summary>
    /// 設置動畫整數
    /// </summary>
    /// <param name="name">名稱</param>
    /// <param name="val">值</param>
    public void SetInteger(string name, int val)
    {
        animator.SetInteger(name, val);
    }
    #endregion 動畫系統基本方法

    #region 動畫觸發事件
    public void StartAttack()
    {
        playerCtrl?.StartAttack();
    }

    public void OnAttack()
    {

    }

    public void EndAttack()
    {
        playerCtrl?.EndAttack();
    }

    public void OpenComboWindow()
    {
        playerCtrl?.OpenComboWindow();
    }
    #endregion 動畫觸發事件
}
