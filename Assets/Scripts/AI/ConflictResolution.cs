using UnityEngine;

public class ConflictResolution : MonoBehaviour
{
    public float timeUntilAction = 0.25f;
    public float smallestAllowedVelocity = 0.09f;

    public float appliedForceMin = -25.0f;
    public float appliedForceMax = 25.0f;

    private string conflictee_;
    private float conflictElapsed_;
    private bool conflictStarted_;

    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsMyTag(other) || conflictStarted_)
        {
            return;
        }

        conflictee_ = other.name;
        conflictStarted_ = true;

        // Disable our motor until the conflict is resolved.
        MovementMotor motor = GetComponent<MovementMotor>();

        if (motor != null)
        {
            motor.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsMyTag(other))
        {
            return;
        }

        if (conflictee_ != other.name)
        {
            return;
        }

        // Apply a bit of force on the other collider (it's doing the same to us!)
        if (other.attachedRigidbody)
        {
            if (Random.Range(1, 100) < 25)
            {
                other.attachedRigidbody.velocity += Vector3.forward * Time.deltaTime * Random.Range(this.appliedForceMin, this.appliedForceMax);
            }
            else if (Random.Range(1, 100) > 50)
            {
                other.attachedRigidbody.velocity += Vector3.left * Time.deltaTime * Random.Range(this.appliedForceMin, this.appliedForceMax);
            }
            else
            {
                other.attachedRigidbody.velocity += Vector3.up * Time.deltaTime * Random.Range(this.appliedForceMin, this.appliedForceMax);
            }
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.rigidbody == null)
        {
            float deltaTime = Time.deltaTime;

            if (Random.Range(1, 100) < 25)
            {
                rigidbody.velocity += Vector3.left * deltaTime * Random.Range(-1, 1) * 100.0f;
            }
            else if (Random.Range(1, 100) > 50)
            {
                rigidbody.angularVelocity += Vector3.forward * deltaTime * Random.Range(-1, 1) * 100.0f;
            }
            else
            {
                rigidbody.velocity += Vector3.left * deltaTime * Random.Range(-1, 1) * 10.0f;
                rigidbody.angularVelocity += Vector3.forward * deltaTime * Random.Range(-1, 1) * 100.0f;
            }

            MovementMotor motor = GetComponent<MovementMotor>();

            if (motor != null)
            {
                motor.enabled = true;
            }
        }
        else
        {
            OnTriggerEnter(collisionInfo.collider);
            OnTriggerStay(collisionInfo.collider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (conflictee_ == other.name)
        {
            conflictee_ = null;
            conflictStarted_ = false;
            conflictElapsed_ = 0.0f;

            MovementMotor motor = GetComponent<MovementMotor>();

            if (motor != null)
            {
                motor.enabled = true;
            }
        }
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (conflictStarted_)
        {
            conflictElapsed_ += Time.deltaTime;

            if (conflictElapsed_ >= this.timeUntilAction)
            {
                // Nudge our motor to a different direction.
                if (Random.Range(1, 100) < 25)
                {
                    rigidbody.velocity += Vector3.forward * conflictElapsed_ * Random.Range(this.appliedForceMin, this.appliedForceMax);
                }
                else if (Random.Range(1, 100) > 50)
                {
                    rigidbody.velocity += Vector3.left * conflictElapsed_ * Random.Range(this.appliedForceMin, this.appliedForceMax);
                }
                else
                {
                    rigidbody.velocity += Vector3.up * conflictElapsed_ * Random.Range(this.appliedForceMin, this.appliedForceMax);
                }

                MovementMotor motor = GetComponent<MovementMotor>();

                if (motor != null)
                {
                    motor.enabled = true;
                }

                conflictElapsed_ = 0.0f;
            }
        }
    }

    private bool IsMyTag(Component other)
    {
        return this.tag == other.tag;
    }
}
