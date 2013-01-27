using UnityEngine;

/// <summary>
/// This class can be used like an interface. Inherit from it to define your own movement motor that can control the
/// movement of characters, enemies, or other entities.
/// </summary>
public class MovementMotor : MonoBehaviour
{
    /// <summary>
    /// The direction the entity wants to move in, in world space. The vector should be normalized.
    /// </summary>
    [HideInInspector]
    public Vector3 movementDirection;

    /// <summary>
    /// Some motors may want to only move towards a target.
    /// </summary>
    [HideInInspector]
    public Vector3 movementTarget;

    /// <summary>
    /// The direction the entity wants to face towards, in world space.
    /// </summary>
    [HideInInspector]
    public Vector3 facingDirection;
}
