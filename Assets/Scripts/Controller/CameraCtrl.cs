using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    #region 鏡頭設定
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    [Range(0f, 20f)]
    private float distance;
    [SerializeField]
    [Range(10f,80f)]
    private float angX;
    [SerializeField]
    [Range(0f,360f)]
    private float angY;
    #endregion 鏡頭設定

    #region 公用參數
    /// <summary>
    /// 角色定位+偏移修正後的最終位置
    /// </summary>
    private Vector3 GPS => GameManager.playerGPS + offset;
    /// <summary>
    /// 是否取得跟隨目標對象
    /// </summary>
    private bool GotTarget => GPS != Vector3.zero;
    #endregion 公用參數

    #region 生命週期
    private void OnEnable()
    {
        GameManager.SetCurrentCamera(this);
    }

    private void OnDisable()
    {
        GameManager.SetCurrentCamera(null);
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }
    #endregion 生命週期

    private void Follow()
    {
        if (!GotTarget) return;
        transform.position = GPS + Angle() * Distance();
        //transform.LookAt(GPS);
    }

    /// <summary>
    /// 組合角度
    /// </summary>
    /// <returns>四元素運算結果</returns>
    private Quaternion Angle()
    {
        return transform.rotation = 
            Quaternion.Euler(angX, angY, 0);
    }

    /// <summary>
    /// 方向向量(距離)
    /// </summary>
    /// <returns>後退長度</returns>
    private Vector3 Distance()
    {
        return Vector3.back * distance;
    }
}
