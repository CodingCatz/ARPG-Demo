using UnityEngine;
//預設必須的元件
[RequireComponent(typeof(CanvasGroup))]
public class UIPanelCtrl : MonoBehaviour
{
    #region 基礎元件
    /// <summary>
    /// CanvasGroup元件本體(盡量不直接控制)
    /// </summary>
    private CanvasGroup _canvasGroup;
    /// <summary>
    /// [延遲載入]CanvasGroup元件
    /// </summary>
    private CanvasGroup canvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
    #endregion 基礎元件

    [Tooltip("UI面板預設是否開啟")]
    public bool openOnAwake;

    void Start()
    {
        Switch(openOnAwake);
    }

    /// <summary>
    /// UI面板切換開關
    /// </summary>
    /// <param name="B">true 開 / flase 關</param>
    public void Switch(bool B)
    {
        canvasGroup.alpha = B ? 1 : 0;
        canvasGroup.blocksRaycasts = B;
    }

    #region ContextMenu測試功能
    [ContextMenu("面板打開")]
    public void PanelOn()
    {
        Switch(true);
    }
    [ContextMenu("面板關閉")]
    public void PanelOff()
    {
        Switch(false);
    }
    #endregion ContextMenu測試功能
}
