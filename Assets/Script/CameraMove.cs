using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
	Vector3 mousePosOld;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButtonDown(1))
		{
			mousePosOld = Input.mousePosition;
		}
		if(Input.GetMouseButton(1))
		{
			Vector3 r = Vector3.Cross(transform.position, Vector3.up);

			transform.RotateAround(Vector3.zero, Vector3.up, (Input.mousePosition.x - mousePosOld.x) * 0.3f);

			float dif = mousePosOld.y - Input.mousePosition.y;

			if (!(dif < 0f && transform.localEulerAngles.x < 20f || dif > 0f && transform.localEulerAngles.x > 80f))
				transform.RotateAround(Vector3.zero, r, dif * 0.3f);

			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
			//transform.Rotate(0f,0f, - transform.localEulerAngles.z, Space.Self);
			mousePosOld = Input.mousePosition;
		}
	}
}
