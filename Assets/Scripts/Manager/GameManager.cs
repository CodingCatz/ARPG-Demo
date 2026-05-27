using System;
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
    /// <summary>
    /// 連結HPBarUI的動作
    /// </summary>
    public static Action<float, float> UpdatePlayerHPBar { get; private set; }

    public static void SetPlayerHPBar(Action<float, float> action)
    {
        UpdatePlayerHPBar += action;
        //玩家已存在的話立刻更新一次
        if (playerCtrl) UpdatePlayerHPBar(playerCtrl.CurrentHP, playerCtrl.MaxHP);
    }
    public static void RemovePlayerHPBar(Action<float, float> action)
    {
        UpdatePlayerHPBar -= action;
    }
    public static void ClearPlayerHPBar()
    {
        UpdatePlayerHPBar = null;
    }
    #endregion 玩家相關資訊

    #region 主攝影機相關
    public static CameraManager mainCamrea { get; private set; }
    public static Vector3 mainCameraRota
    {
        get
        {
            return mainCamrea != null
                ? mainCamrea.transform.rotation.eulerAngles
                : Vector3.zero;
        }
    }
    public static void SetMainCamera(CameraManager main)
    {
        mainCamrea = main;
    }
    #endregion 主攝影機相關

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
