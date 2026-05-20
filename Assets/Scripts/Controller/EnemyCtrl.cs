using UnityEngine;

public class EnemyCtrl : BaseCtrl
{
    #region AI參數
    private Transform _target;
    private Vector2 _aiMoveInput;
    [SerializeField]
    private float _chaseRange = 10f;
    [SerializeField]
    private float _attackRange = 2f;
    #endregion AI參數

    public Transform Target => _target ??= GameManager.playerCtrl.transform;
    public override Vector2 MoveInput => _aiMoveInput;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
