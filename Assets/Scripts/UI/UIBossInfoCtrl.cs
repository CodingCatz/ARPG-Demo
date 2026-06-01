using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIBossInfoCtrl : MonoBehaviour
{
    #region 基礎元件
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private Image _hpBarImg;
    [SerializeField]
    private TextMeshProUGUI _hpBarText;

    private float _HP;
    private float _maxHP;
    private string StrHP => $"{(int)_HP}/{(int)_maxHP}";
    private float PercentHP => _HP / _maxHP;
    #endregion 基礎元件

    #region 生命週期
    private void OnEnable()
    {
        GameManager.SetBossHPBar(UpdateHPBar);
    }

    private void OnDisable()
    {
        GameManager.RemoveBossHPBar(UpdateHPBar);
    }


    #endregion 生命週期

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
