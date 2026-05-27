using UnityEngine;
using Unity.Cinemachine;

public class RoomCtrl : MonoBehaviour
{
    #region 基礎元建
    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    #endregion 基礎元建
    private const string Tag = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag))
        {
            cinemachineCamera.PreviousStateIsValid = true;
        }
    }
}
