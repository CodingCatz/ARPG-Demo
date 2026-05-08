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
    #endregion 基礎元建

    void Start()
    {
        
    }

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
}
