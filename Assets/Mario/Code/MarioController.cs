using UnityEngine;
using System.Collections;

public class MarioController : MonoBehaviour
{

	public Animator animator;

	private Transform defaultCamTransform;
	private Vector3 resetPos;
	private Quaternion resetRot;
	private GameObject cam;
	private GameObject fighter;
	public float speed = 1.0f;

	void Start()
	{
		cam = GameObject.FindWithTag("MainCamera");
		defaultCamTransform = cam.transform;
		resetPos = defaultCamTransform.position;
		resetRot = defaultCamTransform.rotation;
		fighter = GameObject.FindWithTag("Player");
		//fighter.transform.position = new Vector3(0, 0, 0);
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			animator.SetBool("Walk Forward", true);
		}
		else
		{
			animator.SetBool("Walk Forward", false);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			animator.SetBool("Walk Backward", true);
		}
		else
		{
			animator.SetBool("Walk Backward", false);
		}

		if (Input.GetKey(KeyCode.RightControl))
		{
			animator.SetTrigger("PunchTrigger");
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.position += transform.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.position -= transform.right * speed * Time.deltaTime;
		}
	}
	/*void OnGUI()
	{
		if (GUI.RepeatButton(new Rect(815, 535, 100, 30), "Reset Scene"))
		{
			defaultCamTransform.position = resetPos;
			defaultCamTransform.rotation = resetRot;
			fighter.transform.position = new Vector3(0, 0, 0);
			animator.Play("Idle");
		}

		if (GUI.RepeatButton(new Rect(25, 20, 100, 30), "Walk Forward"))
		{
			animator.SetBool("Walk Forward", true);
		}
		else
		{
			animator.SetBool("Walk Forward", false);
		}

		if (GUI.RepeatButton(new Rect(25, 50, 100, 30), "Walk Backward"))
		{
			animator.SetBool("Walk Backward", true);
		}
		else
		{
			animator.SetBool("Walk Backward", false);
		}

		if (GUI.Button(new Rect(25, 90, 100, 30), "Punch"))
		{
			animator.SetTrigger("PunchTrigger");
		}
	}*/
}