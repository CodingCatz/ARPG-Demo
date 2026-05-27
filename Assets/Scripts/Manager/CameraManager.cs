using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.SetMainCamera(this);
    }

    private void OnDisable()
    {
        GameManager.SetMainCamera(null);
    }

    private void OnDestroy()
    {
        GameManager.SetMainCamera(null);
    }
}
