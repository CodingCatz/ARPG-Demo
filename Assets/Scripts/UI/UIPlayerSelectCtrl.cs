using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

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
        /// 選項開關
        /// </summary>
        /// <param name="B">開/關</param>
        public void Switch(bool B)
        {
            vCam.Priority.Enabled = B;
            toggle.isOn = B;
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
        //選中預設第一位
        charOptions[index].Switch(true);
    }

    /// <summary>
    /// 下一個角色(鏡頭逆轉)
    /// </summary>
    public void NextPlayer()
    {
        charOptions[index].Switch(false);
        index++;//索引增加
        charOptions[index].Switch(true);
    }
    /// <summary>
    /// 上一個角色(鏡頭順轉)
    /// </summary>
    public void PrevPlayer() 
    {
        charOptions[index].Switch(false);
        index--;//索引減少
        charOptions[index].Switch(true);
    }
}
