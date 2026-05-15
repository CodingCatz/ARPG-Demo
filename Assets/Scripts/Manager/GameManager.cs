using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 靜態唯一資料管理腳本
/// </summary>
public static class GameManager
{
    #region 玩家相關資訊
    /// <summary>
    /// 當前正再操作角色的索引號碼
    /// </summary>
    public static int playerIndex;
    /// <summary>
    /// 當前正在操作的角色
    /// </summary>
    public static PlayerCtrl playerCtrl {  get; private set; }
    /// <summary>
    /// 當前玩家的座標定位
    /// </summary>
    public static Vector3 playerGPS
    {
        get
        {
            return playerCtrl != null 
                ? playerCtrl.transform.position
                : Vector3.zero;
        }
    }
    /// <summary>
    /// 設定(初始化)當前操作角色
    /// </summary>
    /// <param name="ctrl">角色控制器</param>
    public static void SetCurrentPlayer(PlayerCtrl ctrl)
    {
        playerCtrl = ctrl;
    }
    #endregion 玩家相關資訊

    #region 攝影機相關資訊
    /// <summary>
    /// 當前運作中攝影機
    /// </summary>
    public static CameraCtrl cameraCtrl { get; private set; }
    /// <summary>
    /// 當前運作中攝影機的旋轉參數
    /// </summary>
    public static Vector3 cameraRota
    {
        get
        {
            return cameraCtrl != null
                ? cameraCtrl.transform.rotation.eulerAngles
                : Vector3.zero;
        }
    }
    /// <summary>
    /// 設定(初始化)當前操作鏡頭
    /// </summary>
    /// <param name="ctrl">鏡頭控制器</param>
    public static void SetCurrentCamera(CameraCtrl ctrl)
    {
        cameraCtrl = ctrl;
    }
    #endregion 攝影機相關資訊

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
