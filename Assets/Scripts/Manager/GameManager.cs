using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 靜態唯一資料管理腳本
/// </summary>
public static class GameManager
{
    /// <summary>
    /// 當前正再操作角色的索引號碼
    /// </summary>
    public static int playerIndex;

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
