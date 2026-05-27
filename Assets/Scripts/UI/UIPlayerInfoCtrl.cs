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
    [SerializeField]
    private Image _hpBarImg;
    [SerializeField]
    private TextMeshProUGUI _hpBarText;

    private float _HP;
    private float _maxHP;
    #endregion 基礎元件

    #region 公用參數
    /// <summary>
    /// 玩家的索引(編號)
    /// </summary>
    private int PlayerIndex => GameManager.playerIndex;
    private string StrHP => $"{(int)_HP}/{(int)_maxHP}";
    private float PercentHP => _HP / _maxHP;
    #endregion 公用參數

    /// <summary>
    /// 程式重啟的時候觸發
    /// </summary>
    private void OnEnable()
    {
        InitialUI();
        GameManager.SetPlayerHPBar(UpdateHPBar);
    }

    private void OnDisable()
    {
        GameManager.RemovePlayerHPBar(UpdateHPBar);
    }

    private void OnDestroy()
    {
        GameManager.RemovePlayerHPBar(UpdateHPBar);
    }

    /// <summary>
    /// 初始化玩家資訊UI
    /// </summary>
    private void InitialUI()
    {
        _headImg.sprite = _playerDB.GetPlayerData(PlayerIndex).icon;
        _nameText.text = _playerDB.GetPlayerData(PlayerIndex).name;
    }

    /// <summary>
    /// 更新血量功能
    /// </summary>
    /// <param name="HP">血量</param>
    /// <param name="maxHP">血量最大值</param>
    private void UpdateHPBar(float HP, float maxHP)
    {
        _HP = HP;
        _maxHP = maxHP;
        _hpBarText.text = StrHP;
        _hpBarImg.fillAmount = PercentHP;
    }
}
