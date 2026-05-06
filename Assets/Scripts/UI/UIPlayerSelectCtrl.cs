using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using static UIPlayerSelectCtrl;

public class UIPlayerSelectCtrl : MonoBehaviour
{
    /// <summary>
    /// 索引原始值
    /// </summary>
    private int _index;
    /// <summary>
    /// 索引操作變數
    /// </summary>
    private int index
    {
        get
        {
            return _index;
        }
        set
        {//小於0特殊處理成尾號，否則取餘數
            _index = value < 0 ? charOptions.Length - 1 : value % charOptions.Length;
        }
    }
    [SerializeField]
    private PlayerDB _playerDB;
    [SerializeField]
    private TextMeshProUGUI _textName;
    [SerializeField]
    private TextMeshProUGUI _textDesc;

    /// <summary>
    /// 角色選項資料結構
    /// </summary>
    [Serializable]
    public struct CharOption
    {
        /// <summary>
        /// 虛擬鏡頭設定
        /// </summary>
        public CinemachineCamera vCam;
        /// <summary>
        /// UI狀態提示(是否選中)
        /// </summary>
        public Toggle toggle;
        /// <summary>
        /// 定位(焦點)
        /// </summary>
        public Transform location;

        /// <summary>
        /// 角色選項開關切換
        /// </summary>
        /// <param name="B">開/關</param>
        public void Switch(bool B)
        {
            vCam.Priority.Enabled = B;
            toggle.isOn = B;
        }

        /// <summary>
        /// 設定Toggle圖案
        /// </summary>
        /// <param name="icon">圖案</param>
        public void SetToggle(Sprite icon)
        {
            (toggle.targetGraphic as Image).sprite = icon;
            (toggle.graphic as Image).sprite = icon;
        }
    }
    /// <summary>
    /// [陣列]角色選項設定集合物件
    /// </summary>
    public CharOption[] charOptions;

    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < charOptions.Length; i++)
        {//起始(0)；終點(3)；迭代(1)
            PlayerData data = _playerDB.GetPlayerData(i);
            charOptions[i].SetToggle(data.icon);
            //具現化物件(預置物，座標，旋轉)
            Instantiate(data.playerCtrl, charOptions[i].location.position, charOptions[i].location.rotation);
        }
        
        //選中預設第一位
        UpdateInfo();
    }

    /// <summary>
    /// 更新選角UI資訊
    /// </summary>
    private void UpdateInfo()
    {
        charOptions[index].Switch(true);
        _textName.text = _playerDB.GetPlayerData(index).name;
        _textDesc.text = _playerDB.GetPlayerData(index).desc;
    }

    /// <summary>
    /// 進入遊戲舞台
    /// </summary>
    public void EnterStage()
    {
        GameManager.playerIndex = index;
        GameManager.LoadScene("Gaming");
    }

    /// <summary>
    /// 下一個角色(鏡頭逆轉)
    /// </summary>
    public void NextPlayer()
    {
        charOptions[index].Switch(false);
        index++;//索引增加
        UpdateInfo();
    }
    /// <summary>
    /// 上一個角色(鏡頭順轉)
    /// </summary>
    public void PrevPlayer() 
    {
        charOptions[index].Switch(false);
        index--;//索引減少
        UpdateInfo();
    }
}
