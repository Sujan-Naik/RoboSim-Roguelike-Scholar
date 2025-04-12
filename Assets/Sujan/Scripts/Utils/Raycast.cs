using UnityEngine;

public static class Raycast
{
    
    public static readonly int NON_CHARACTER_MASK = ~LayerMask.GetMask("Character", "Ignore Raycast");
    public static readonly int CHARACTER_MASK = LayerMask.GetMask("Character");

    
    public static Collider[] SphereOverlap(Vector3 position, float radius, LayerMask mask)
    {
        return Physics.OverlapSphere(position, radius, mask);
    }
    
    public static Collider[] EnvironmentOverlap(Vector3 position, float radius)
    {
        return Physics.OverlapSphere(position, radius, NON_CHARACTER_MASK);
    }
    
    public static Collider[] CharacterOverlap(Vector3 position, float radius)
    {
        return Physics.OverlapSphere(position, radius, CHARACTER_MASK);
    }
    
    public static bool SphereRaycast(Vector3 position, float radius, Vector3 direction, out RaycastHit hitInfo, float distance, LayerMask mask)
    {
        return Physics.SphereCast(position, radius, direction, out hitInfo, distance, mask );
    }

    public static bool NonCharacterRaycast(Vector3 position, Vector3 forward, out RaycastHit hitInfo, float range)
    {
        return Physics.Raycast(position, forward, out hitInfo,
           range, NON_CHARACTER_MASK, QueryTriggerInteraction.Collide);
    }

    public static bool EnvironmentRaycast(Vector3 position, float radius, Vector3 forward, out RaycastHit hitInfo,
        float range)
    {
        return SphereRaycast(position, radius, forward, out hitInfo, range, NON_CHARACTER_MASK);
    }

    public static bool CharacterRaycast(Vector3 position, float radius, Vector3 forward, out RaycastHit hitInfo, float range)
    {
        return SphereRaycast(position, radius, forward, out hitInfo, range, CHARACTER_MASK);
    }
}