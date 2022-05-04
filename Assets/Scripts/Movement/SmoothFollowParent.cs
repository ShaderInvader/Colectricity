using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowParent : MonoBehaviour
{
	Transform target;
	public Vector3 offset;
	public float smoothSpeed = 0.08f;

    void Start()
    {
		target = transform.parent;
		transform.SetParent(target.parent, true);
	}

    void FixedUpdate()
	{
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed);
		transform.position = smoothedPosition;
	}
}
