using UnityEngine;

public class Entity_AnimationEvents : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void DamageEntity() => entity.DamageTargets();

    private void EnableJumpAndMovement()
    {
        entity.EnableJumpAndMovement(true);
        entity.IsAttackingStop();
    }

    private void DisableJumpAndMovement()
    {
        entity.EnableJumpAndMovement(false);
    }
}