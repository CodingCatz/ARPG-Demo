using UnityEngine;
using Unity.Cinemachine;

public class SkillCtrl : MonoBehaviour
{
    #region 基礎元建
    /// <summary>
    /// 鏡頭震動元件本體
    /// </summary>
    private CinemachineImpulseSource _impulseSource;
    private CinemachineImpulseSource impulseSource
    {
        get 
        { 
            if (_impulseSource == null)
            {
                _impulseSource = GetComponent<CinemachineImpulseSource>();
                if (_impulseSource == null )
                    _impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
            }
            return _impulseSource; 
        }
    }

    #endregion 基礎元建

    public enum Target { None, Enemy, Player }
    [SerializeField]
    private Target target;
    [SerializeField]
    private GameObject _hitEffectObj;
    [SerializeField]
    private float _hitPower = 0f;
    private bool HitShock => _hitPower > 0f;
    [SerializeField]
    private float _destroyTime = 2f;
    private string Tag
    {
        get
        {
            switch (target)
            {
                case Target.Enemy: return "Enemy";
                case Target.Player: return "Player";
            }
            return string.Empty;
        }
    }
    
    void Start()
    {
        Destroy(gameObject, _destroyTime);
    }

    /// <summary>
    /// 物件上必須要有碰撞器，且勾上IsTrigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag)
        {
            _hitEffectObj.SetActive(true);
            if (HitShock) impulseSource.GenerateImpulseWithForce(_hitPower);
        }
    }
}
