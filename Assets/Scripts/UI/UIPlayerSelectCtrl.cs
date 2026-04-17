using UnityEngine;
using Unity.Cinemachine;

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
            _index = value < 0 ? vCams.Length - 1 : value % vCams.Length;
        }
    }
    /// <summary>
    /// [陣列]虛擬鏡頭設定集合物件
    /// </summary>
    public CinemachineCamera[] vCams;

    /// <summary>
    /// 下一個角色(鏡頭逆轉)
    /// </summary>
    public void NextPlayer()
    {
        vCams[index].Priority.Enabled = false;
        index++;//索引增加
        vCams[index].Priority.Enabled = true;
    }
    /// <summary>
    /// 上一個角色(鏡頭順轉)
    /// </summary>
    public void PrevPlayer() 
    {
        vCams[index].Priority.Enabled = false;
        index--;//索引減少
        vCams[index].Priority.Enabled = true;
    }
}
