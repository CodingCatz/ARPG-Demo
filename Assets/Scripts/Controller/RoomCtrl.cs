using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class RoomCtrl : MonoBehaviour
{
    #region 基礎元建
    [SerializeField]
    private CinemachineCamera cinemachineCamera;
    [SerializeField]
    private PlayableDirector director;
    [SerializeField]
    private Collider doorBlock;
    [SerializeField]
    private BossCtrl bossCtrl;
    #endregion 基礎元建
    private const string Tag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag))
        {
            director.Play();
            doorBlock.isTrigger = false;
            cinemachineCamera.Priority.Value = 100;
            _ = bossCtrl.Ready((float)director.duration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag))
        {
            cinemachineCamera.Priority.Value = 0;
        }
    }
}
