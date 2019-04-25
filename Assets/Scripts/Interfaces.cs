using UnityEngine;

public interface IDamageable
{
    void TakeHit(float damage, RaycastHit2D hit);
}

public interface ITakeDamage
{
    void PlayerTakeHit(float damage, Transform target);
}

public interface IPowerUp
{
    void AddHealth(float amount);
    void Shrink(Vector3 vector, Vector2 size, Vector2 boxOffset, Vector2 circleOffset, Transform target, BoxCollider2D collider, CircleCollider2D circle);
    void UnShrink(Vector3 vector, Vector2 size, Vector2 boxOffset, Vector2 circleOffset, Transform target, BoxCollider2D collider, CircleCollider2D circle);
    void ExtraDamage(float amount, Transform target);
}

public interface ISpawnPowerup
{
    void SpawnPowerup();
}

public interface IShootAmmo
{
    void DepleteAmmo(Weapon weapon);
}