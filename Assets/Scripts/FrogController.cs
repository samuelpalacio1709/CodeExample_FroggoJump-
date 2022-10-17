using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class FrogController : MonoBehaviour
{

    [SerializeField] float forceMultiplier = 2f;
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    private Rigidbody rb;
    private Vector3 apliedForce;
    private float actualSpeed;
    public Vector3 playerPos;

    private bool isShoot;
    bool canJump = true;
    float xMax;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void OnMouseDown()
    {
        if (canJump)
        {  
            mousePressDownPos = Input.mousePosition;
        }
    }
    private void OnMouseDrag()
    {
        if (canJump)
        {
            apliedForce = (mousePressDownPos - Input.mousePosition) * 0.01f * forceMultiplier;
            Vector3 endPos = GetEndPoint(apliedForce.magnitude);
            actualSpeed = PhysicsFormulas.GetInitialVelocity(endPos, this.gameObject);
        }
    }

    private void OnMouseUp()
    {
        if (canJump)
        {
                mouseReleasePos = Input.mousePosition;
                Shoot(actualSpeed, PhysicsFormulas.GetAngle(mousePressDownPos, mouseReleasePos));       
        }
    }

    public void Shoot(float vo, float a)
    {
        if (isShoot) return;
        canJump = false;

        if (apliedForce.y >= 0)
        {
            canJump = false;
            rb.transform.eulerAngles = new Vector3(this.transform.rotation.x, a, this.transform.rotation.z);
            rb.velocity = (transform.TransformDirection(new Vector3(0, (vo) * Mathf.Sin(45), (vo) * Mathf.Cos(45))));
            rb.transform.eulerAngles = new Vector3(this.transform.rotation.x, 0, this.transform.rotation.z);
            apliedForce = Vector3.zero;
            actualSpeed = 0;
        }

    }

    public Vector3 GetEndPoint(float v0)
    {
        Vector3 endPos;
        xMax = (-v0 * PhysicsFormulas.GetTime(v0)) / 2;
        endPos = (new Vector3((transform.forward.x * xMax - 0.2f) + this.transform.position.x, transform.forward.y + 0.65f, (transform.forward.z * xMax - 0.2f) + this.transform.position.z));

        return endPos;
    }

}
