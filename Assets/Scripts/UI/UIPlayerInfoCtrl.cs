using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoCtrl : MonoBehaviour
{
    #region 基礎元件
    [SerializeField]
    private PlayerDB _playerDB;
    [SerializeField]
    private Image _headImg;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    #endregion 基礎元件

    #region 公用參數
    /// <summary>
    /// 玩家的索引(編號)
    /// </summary>
    private int PlayerIndex => GameManager.playerIndex;
    #endregion 公用參數

    /// <summary>
    /// 程式重啟的時候觸發
    /// </summary>
    private void OnEnable()
    {
        InitialUI();
    }

    /// <summary>
    /// 初始化玩家資訊UI
    /// </summary>
    private void InitialUI()
    {
        _headImg.sprite = _playerDB.GetPlayerData(PlayerIndex).icon;
        _nameText.text = _playerDB.GetPlayerData(PlayerIndex).name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
