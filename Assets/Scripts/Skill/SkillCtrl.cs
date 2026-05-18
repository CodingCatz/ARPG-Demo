using UnityEngine;

public class SkillCtrl : MonoBehaviour
{
    public enum Target { None, Enemy, Player }
    [SerializeField]
    private Target target;
    [SerializeField]
    private GameObject _hitEffectObj;
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
        }
    }
}
