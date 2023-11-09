# CodeExample_FroggoJump-

Hey there! In this section, I'll explain in detail how the jump in the game I was part of, FroggoJump!, works. Basically, I use physics formulas for projectile motion and some trigonometry formulas.

![ezgif com-gif-maker_(1)](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/d860f084-8bb1-4c77-9cfe-58a3fed70339)

### Setup inicial

I'll recreate the functionality with an example from scratch.

![Untitled](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/a0aa7142-428f-4d79-b677-af970f6e406d)

![Untitled 1](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/c57169a7-4376-4e8f-8dfa-c31f8ccaf6d0)


```csharp
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class FrogController : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        TryGetComponent(out rb);
    }
}

```

These are the necessary variables that I'll explain shortly.

```csharp
[SerializeField] float forceMultiplier = 2f;
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    private Vector3 appliedForce;
    private Rigidbody rb;
    private float initialVelocity;
    private bool canJump = true;
```

For the game input, we'll use three Unity callbacks: OnMouseDown, OnMouseDrag, and OnMouseUp.

In OnMouseDown, we save the mouse or finger position at the moment it touches the frog.

```csharp
//This method is called once the player touches the frog.
private void OnMouseDown()
    {
        if (canJump)
        {
            //Save the mouse position when the player touches the frog 
            mousePressDownPos = Input.mousePosition;          
        }
    }
```

Now, we want the frog to have a physically accurate jump, so I use basic formulas for projectile motion. To keep the FrogController script from getting too long, I opted to create a class called PhysicsFormulas that stores all these formulas in static methods.
The default jump angle will be set to 45 degrees.

```csharp
public class PhysicsFormulas 
{
	
	 public static float angle=45; 
}
```

Note that the PhysicsFormulas class doesn't need to inherit from MonoBehaviour.

### Initial Velocity

![Untitled 2](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/3a67d78c-c12a-4b61-ae75-e68281c87ec3)


This method returns the initial velocity that the frog needs to reach a given position.

```csharp
 public static float GetInitialVelocity(Vector3 pos, GameObject frog)
    {
        float initialVelocity;
        Vector3 target = pos;
        float distance = Vector3.Distance(frog.transform.position, target);
        initialVelocity = Mathf.Sqrt((distance * 9.81f) / Mathf.Sin(2 * angle));
        return initialVelocity;
    }

   
```

### **Time**

![Untitled 3](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/61dca840-3b01-49f0-a02d-f123fb23fe1f)

It calculates the time it takes for the frog to cover a distance, given an initial velocity.

```csharp
public static float GetTime(float vo)
    {
        return Mathf.Abs(((2f * vo * Mathf.Sin(angle)) / Physics.gravity.y));
    }
```


## Farthest point

![Untitled 4](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/7cf22202-57d4-4633-824d-274291bc1ed1)


With this formula, we find the maximum distance. We multiply this distance by the frog's position to obtain a Vector3 for the final jump position.
```csharp

public static Vector3 GetEndPoint(float v0, Transform frogTransform)
    {
        float maxDistance = 0;
        Vector3 endPos;
        maxDistance = (v0 * PhysicsFormulas.GetTime(v0));
        endPos = (new Vector3((frogTransform.forward.x * maxDistance ) 
          + frogTransform.position.x, frogTransform.forward.y , 
          (frogTransform.forward.z * maxDistance) + frogTransform.position.z));

        return endPos;
    }

```

## Highest point

![Untitled 5](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/71329862-1f1a-476f-ac8d-ab5cdd4304dc)


To find the midpoint between the frog and the final point, it's easily achieved with a Lerp. If we interpolate both vectors to 50% (0.5), we get the midpoint between those 2 vectors. When we add the "Y" value with the maximum height, we obtain the highest point. This works because the midpoint of the total distance in a projectile motion is always the highest point
```csharp
public static Vector3 GetHighestPoint(float v0, Transform frogTransform)
    {
        Vector3 pos = GetEndPoint(v0, frogTransform);
        Vector3 point = Lerp(frogTransform.position, pos, 0.5f);
        float y = (frogTransform.position.y
           + (-Physics.gravity.y * Mathf.Pow(PhysicsFormulas.GetTime(v0), 2)) / 2);

        return new Vector3(point.x, y, point.z);
    }
    public static Vector3 Lerp(Vector3 start, Vector3 end, float percentage)
    {
        return (start + percentage * (end - start));
    }
```
![Untitled 6](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/4875b122-e2cf-404a-bb78-a2053f4447ea)


Now, let's continue with the OnMouseDrag callback where I'll call these methods.

```csharp
private void OnMouseDrag()
    {
        if (canJump)
        {
    //The distance between the finger and the frog
            appliedForce = (mousePressDownPos - Input.mousePosition) * forceMultiplier;
        
    //We get the expected position if the frog were launched with that force
            Vector3 endPos = PhysicsFormulas.GetEndPoint(appliedForce.magnitude, this.transform);
            
    //Here, we get the velocity needed to get to that point
           initialVelocity = PhysicsFormulas.GetInitialVelocity(endPos, this.gameObject);

            if (appliedForce.y >= 0.5)	//We need the player to not be able to jump inside the same water lily

            {
    // The path script needs the highest point to draw the linerenderer
                Vector3 highestPoint = PhysicsFormulas.GetHighestPoint(appliedForce.magnitude, this.transform); 
                Path.Activate(endPos,highestPoint);

    // We need to know where the finger es pointing, so we get the angle between
        //the mousePressDownPos and the current finger position
                float angle  = PhysicsFormulas.GetAngle(mousePressDownPos, Input.mousePosition);
    // Rotate the frog rigidbody, so the this one is pointing to the endposition
         rb.transform.eulerAngles = new Vector3(this.transform.rotation.x, angle, this.transform.rotation.z);

            }
        }
    }
```

The force applied to the frog is proportional to the distance between the finger and the frog, thanks to subtracting the finger's position when it touched the frog from the current finger position.
![Untitled 7](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/d6bb8d8d-9931-4c52-9c29-9ba275c3dee5)

We have a force being applied to the frog, now we need to know what the endpoint (Vector3) would be if it is launched with this force. Once we have the endPoint, we can exactly determine what the initial velocity should be for it to reach that endPoint.
```csharp
  Vector3 endPos = PhysicsFormulas.GetEndPoint(appliedForce.magnitude, this.transform);
            initialVelocity = PhysicsFormulas.GetInitialVelocity(endPos, this.gameObject);
```

---



We need to rotate the frog in the y-axis to the angle that the finger is pointing. To do this, we use trigonometry.
```csharp
  if (appliedForce.y >= 0.5)
            {
                Path.active = true;
                Path.endPosition = endPos;
                Path.highestPoint = PhysicsFormulas.GetHighestPoint(appliedForce.magnitude, this.transform);
								
                float angle = PhysicsFormulas.GetAngle(mousePressDownPos, Input.mousePosition);
                transform.eulerAngles = new Vector3(this.transform.rotation.x, angle, this.transform.rotation.z);
            }
```

![Untitled 8](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/6a22f065-e578-4d06-8a92-3c86c4fee4eb)

In code it would look like this
```csharp
public static float GetAngle(Vector3 mousePressDownPos, Vector3 mouseReleasePos)
    {

        float signedAngle;
        float adjacentSide = mousePressDownPos.y - mouseReleasePos.y;
        float oppositeSide = Mathf.Abs(mouseReleasePos.x - (Screen.width / 2));

        if (mouseReleasePos.x > (Screen.width / 2))
        {
            signedAngle = -(Mathf.Atan2(h, a) * Mathf.Rad2Deg);
        }
        else
        {
            signedAngle = (Mathf.Atan2(h, a) * Mathf.Rad2Deg);

        }

        return signedAngle;
    }
```

Note that if the finger is in the first half of the screen, the angle is positive, and if it's in the other half, it's negative.

Finally, we use the OnMouseUp callback, where we make the frog jump.

```csharp
private void OnMouseUp()
    {
        if (canJump)
        {
						
           canJump = false;
          mouseReleasePos = Input.mousePosition;

          // We need the frog to be able to jump again when it has reached the end pos.
          // Previously, I explained how we can get the total flying time using a simple physic formula
          StartCoroutine(ActivateJump(PhysicsFormulas.GetTime(initialVelocity)));
          //Let's make the frog jump!       
					Jump(initialVelocity);
          Path.Deactivate();
        }
    }
```

```csharp
public void Jump(float vo)
    {
        //we add the initial velocity decomposed into z and y
       rb.velocity = (transform.TransformDirection(new Vector3(0, (vo) * Mathf.Sin(45), (vo) * Mathf.Cos(45))));
       rb.transform.eulerAngles = new Vector3(this.transform.rotation.x, 0, this.transform.rotation.z);
    }
```

Using trigonometry, the velocity must be decomposed into x, y, and z components. Note that the force is being applied as if it were in 2 dimensions, since x = 0. However, with the TransformDirection method, we can make local coordinates become global. So, by rotating the frog, we are "rotating the frog's global axis."

![Vdeo_en_negro_2](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/06512919-e057-4750-857c-4fb99cd90141)


For the camera to follow the player, I made a simple script for this example using Lerp:
```csharp
public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float zOffest=6;
    [SerializeField] float speed=4;
    Vector3 target;
    private void Update()
    {
        //Simple linear interpolation to follow the frog 
        target = new Vector3(player.position.x, this.transform.position.y, player.position.z - zOffest);
        transform.position = Vector3.Lerp(transform.position,target , speed * Time.deltaTime);
    }
}
```

---

Now, when will the canJump bool become true again?

Using the formula to get the flight time, we can know how long it takes for the frog to arrive. We call a coroutine to wait for that time and be able to jump again.

```csharp
  StartCoroutine(ActivateJump(PhysicsFormulas.GetTime(initialVelocity)));

```

```csharp

IEnumerator ActivateJump(float time)
    {
        yield return new WaitForSeconds(time);
        canJump = true;
    }
```


![ezgif com-gif-maker_(1)](https://github.com/samuelpalacio1709/CodeExample_FroggoJump-/assets/82546723/d860f084-8bb1-4c77-9cfe-58a3fed70339)




