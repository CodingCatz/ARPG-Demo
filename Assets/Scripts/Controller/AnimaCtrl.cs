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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


}
