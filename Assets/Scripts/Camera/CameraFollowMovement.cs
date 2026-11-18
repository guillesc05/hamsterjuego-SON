using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMovement : MonoBehaviour
{
    [SerializeField] private Transform _hamsterCenter; //El objeto center del hamster

    private void FixedUpdate()
    {
        Vector3 camPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetPosition = new Vector3(_hamsterCenter.position.x, _hamsterCenter.position.y, transform.position.z);

        transform.position = 
            Vector3.Lerp(
                camPosition, 
                targetPosition, 
                4f * Time.deltaTime);
    }
}
