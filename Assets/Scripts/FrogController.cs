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
    private Vector3 apliedForce;
    private Rigidbody rb;
    private float initialVelocity;
    private bool canJump = true;

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
        
            Vector3 endPos = PhysicsFormulas.GetEndPoint(apliedForce.magnitude, this.transform);
            initialVelocity = PhysicsFormulas.GetInitialVelocity(endPos, this.gameObject);

            if (apliedForce.y >= 0.5)
            {
              
                Vector3 highestPoint = PhysicsFormulas.GetHighestPoint(apliedForce.magnitude, this.transform);
                Path.Activate(endPos,highestPoint);

                float angle  = PhysicsFormulas.GetAngle(mousePressDownPos, Input.mousePosition);
                rb.transform.eulerAngles = new Vector3(this.transform.rotation.x, angle, this.transform.rotation.z);


            }
        }
    }

    private void OnMouseUp()
    {
        if (canJump)
        {
            if (apliedForce.magnitude >= 4f && ((mousePressDownPos.y - 0.5f) >= mouseReleasePos.y))
            {
                canJump = false;
                mouseReleasePos = Input.mousePosition;
                StartCoroutine(ActivateJump(PhysicsFormulas.GetTime(initialVelocity)));
                Jump(initialVelocity);
            }


            Path.Deactivate();

        }
    }

    public void Jump(float vo)
    {
       rb.velocity = (transform.TransformDirection(new Vector3(0, (vo) * Mathf.Sin(45), (vo) * Mathf.Cos(45))));
       rb.transform.eulerAngles = new Vector3(this.transform.rotation.x, 0, this.transform.rotation.z);
        
    }

    IEnumerator ActivateJump(float time)
    {
        yield return new WaitForSeconds(time);
        rb.velocity = Vector3.zero;
        canJump = true;
    }
}
