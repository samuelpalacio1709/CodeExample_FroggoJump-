using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float zOffest;
    [SerializeField] float speed=4;
    Vector3 target;
    private void Update()
    {
        target = new Vector3(player.position.x, this.transform.position.y, player.position.z - zOffest);
        transform.position = Vector3.Lerp(transform.position,target , speed * Time.deltaTime);
    }
}
