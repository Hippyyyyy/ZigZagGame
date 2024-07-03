using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] ChangerBackgroundColor changerBackground;

    Vector3 distance;

    [SerializeField] float smoothValue;

    public Transform Target { get => target; set => target = value; }
    public ChangerBackgroundColor ChangerBackground { get => changerBackground; set => changerBackground = value; }

    private void Start()
    {
        distance = Target.position - transform.position;
    }

    private void Update()
    {
        if (Target.position.y >= 0)
        {
            Follow();
        }
    }

    public void Follow()
    {
        Vector3 currentPos = transform.position;

        Vector3 targetPos = Target.position - distance;

        transform.position = Vector3.Lerp(currentPos, targetPos, smoothValue * Time.deltaTime);
    }
}
