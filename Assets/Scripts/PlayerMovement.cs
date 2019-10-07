using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	CharacterController cc;
	Animator ac;
	Vector3 moveVec, gravity;
	public GameObject GM;
	public float	speed = 5,
					JumpSpeed = 12;
	public bool CanPlay;
	
	int laneNumber = 1,
		lanesCount = 2;

	public float FirstLanePos,
		LaneDistance,
		SideSpeed;

	bool isRolling = false;

	Vector3 ccCenterNorm = new Vector3(0, .96f, 0);
	Vector3 ccCenterRoll = new Vector3(0, .4f, 0);

	float ccHeightNorm = 2;
	float ccHeightRoll = 0.4f;
	void Start()
    {
	    cc = GetComponent<CharacterController>();
	    ac = GetComponent<Animator>();
	    moveVec = new Vector3(1,0,0);
	    gravity = Vector3.zero;
    }

    void Update()
    {
	    if (cc.isGrounded)
	    {
		    gravity = Vector3.zero;

		    if (CanPlay)
			    if (!isRolling)
			    {
				    if (Input.GetAxisRaw("Vertical") > 0)
				    {
					    ac.SetTrigger("jumping");
					    gravity.y = JumpSpeed;
				    }
				    else if (Input.GetAxisRaw("Vertical") < 0)
					    StartCoroutine(DoRoll());
			    }
	    }
	    else
		    gravity += Physics.gravity * Time.deltaTime * 3;

	    if (cc.velocity.y < 0)
		    ac.SetTrigger("falling");
	    if (CanPlay)
		    moveVec.x = speed;
	    moveVec += gravity;
	    moveVec *= Time.deltaTime;

	    CheckInput();

	    cc.Move(moveVec);
	    Vector3 newPos = transform.position;
	    newPos.z = Mathf.Lerp(newPos.z, FirstLanePos + (laneNumber * LaneDistance), Time.deltaTime * SideSpeed);
		    transform.position = newPos;
    }

    void CheckInput()
    {
	    int sign;

	    if (!CanPlay || isRolling)
		    return;

	    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		    sign = -1;
	    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		    sign = 1;
	    else
			return;
	    laneNumber += sign;
	    laneNumber = Mathf.Clamp(laneNumber, 0, lanesCount);
    }

    IEnumerator DoRoll()
    {
	    isRolling = true;
	    ac.SetBool("rolling", true);
	    cc.center = ccCenterRoll;
	    cc.height = ccHeightRoll;
	    yield return new WaitForSeconds(1.5f);
	    isRolling = false;
	    ac.SetBool("rolling", false);
	    cc.center = ccCenterNorm;
	    cc.height = ccHeightNorm;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
	    GM.SetActive(true);
	    /*if (!hit.gameObject.CompareTag("Trap"))
		    return;
	    StartCoroutine(Death());*/
    }

    IEnumerator Death()
    {
	    CanPlay = false;
	    yield return new WaitForSeconds(2);
	    GM.SetActive(true);
    }
}
