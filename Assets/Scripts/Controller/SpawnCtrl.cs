using UnityEngine;

/// <summary>
/// 角色誕生控制
/// </summary>
public class SpawnCtrl : MonoBehaviour
{
    #region 基礎元件
    [SerializeField]
    private PlayerDB _playerDB;
    #endregion 基礎元件

    #region 公用參數
    /// <summary>
    /// 玩家的索引(編號)
    /// </summary>
    private int PlayerIndex => GameManager.playerIndex;
    private PlayerCtrl CurrentPlayer => _playerDB.GetPlayerData(PlayerIndex).playerCtrl;
    #endregion 公用參數

    /// <summary>
    /// 誕生控制初始
    /// </summary>
    void Start()
    {
        Spawn();
    }

    /// <summary>
    /// 產生對應選取的角色
    /// </summary>
    private void Spawn()
    {
        Instantiate(CurrentPlayer, transform.position, transform.rotation);
    }
}
