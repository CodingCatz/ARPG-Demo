using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region 基礎元件
    [SerializeField]
    private GameObject _enemyPrefab;
    #endregion 基礎元件

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Instantiate(_enemyPrefab, transform.position, transform.rotation);
    }
}
