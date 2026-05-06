using System;
using UnityEngine;

/// <summary>
/// 角色基本資料
/// </summary>
[Serializable]
public struct PlayerData
{
    /// <summary>
    /// 名稱
    /// </summary>
    public string name;
    /// <summary>
    /// 說明描述
    /// </summary>
    [TextArea]
    public string desc;
    /// <summary>
    /// 代表性圖標
    /// </summary>
    public Sprite icon;
    /// <summary>
    /// 玩家控制模組
    /// </summary>
    public PlayerCtrl playerCtrl;
}

/// <summary>
/// 角色資料庫
/// </summary>
[CreateAssetMenu(fileName = "PlayerDB", menuName = "DataBase/PlayerDB")]
public class PlayerDB : ScriptableObject
{
    public PlayerData[] datas;

    /// <summary>
    /// 使用索引值取得角色資料
    /// </summary>
    /// <param name="index">索引值</param>
    /// <returns>角色資料</returns>
    public PlayerData GetPlayerData(int index)
    {
        return datas[index];
    } 
}
