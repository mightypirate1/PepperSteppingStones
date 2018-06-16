﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperAgentSecond : Agent {

    public float ArenaDimensions = 20.0f;
    public float speed = 1f;
    public float rotation_speed = 0.3f;
    // What the agent is chasing
    public Transform Target;
    public GameObject cam;

    Rigidbody rBody;

    private float previousDistance = float.MaxValue;

    private Rigidbody agentRigidbody;
    private Vector3 offset = new Vector3(0.0f,1.0f,0.0f);

    private float moveSpeed;
    private float turnSpeed;

    private int maxStepsPerEpoch;
    private int steps;

    void MoveCamera()
    {
        // Vector3 offset = new Vector3(0.0f,1.0f,0.0f);
        Vector3 offset = 1f*rBody.transform.up + 0.1f*rBody.transform.forward;
        this.cam.transform.position = transform.position + offset;
        this.cam.transform.forward = rBody.transform.forward;
    }

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void InitializeAgent()
    {
      this.cam = GameObject.Find("Main Camera");
      //agentParameters.agentCameras.Add( cam.GetComponent<Camera>() );
    }

    public override void AgentReset()
    {
        float allowedArea = ArenaDimensions * 0.5f;

        // Move the rat to a new spot
        //this.rBody.position = new Vector3((Random.value * allowedArea) - (allowedArea / 2),
        //                                0.16f,
        //                                (Random.value * allowedArea) - (allowedArea / 2));

	      this.transform.position = new Vector3((Random.value * allowedArea) - (allowedArea / 2),
                                        0.16f,
                                        (Random.value * allowedArea) - (allowedArea / 2));

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;

        // Move the target to a new spot
        Target.position = new Vector3((Random.value * allowedArea) - (allowedArea / 2),
                                        0.1f,
                                        (Random.value * allowedArea) - (allowedArea / 2));

        MoveCamera();

	      // Custom speed
 	      //this.agentRigidbody = GetComponent<Rigidbody>();
        //this.moveSpeed = 0.5f;
        //this.turnSpeed = 400f;
	      this.maxStepsPerEpoch = 1000;
    }

    public override void CollectObservations()
    {

        // float arenaEdgefromCenter = ArenaDimensions / 2;
        //
        // // Calculate relative position
        // Vector3 relativePosition = Target.position - this.transform.position;
        //
        // // Relative position
        // AddVectorObs(relativePosition.x / arenaEdgefromCenter);
        // AddVectorObs(relativePosition.z / arenaEdgefromCenter);
        //
        // // Distance to edges of platform
        // AddVectorObs((this.transform.position.x + arenaEdgefromCenter) / arenaEdgefromCenter);
        // AddVectorObs((this.transform.position.x - arenaEdgefromCenter) / arenaEdgefromCenter);
        // AddVectorObs((this.transform.position.z + arenaEdgefromCenter) / arenaEdgefromCenter);
        // AddVectorObs((this.transform.position.z - arenaEdgefromCenter) / arenaEdgefromCenter);
        //
        // // Agent velocity
        // AddVectorObs(rBody.velocity.x / arenaEdgefromCenter);
        // AddVectorObs(rBody.velocity.z / arenaEdgefromCenter);

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        // Reached target
        if (distanceToTarget < 0.5f)
        {
            Done();
            AddReward(1.0f);
        }

        // Getting closer
        if (distanceToTarget < previousDistance)
        {
            AddReward(0.1f);
        }


	      // // Set orientation
        // Vector3 deltaPosition = Target.position - transform.position;
        //
        // if (deltaPosition != Vector3.zero) {
        //     // Same effect as rotating with quaternions, but simpler to read
        //     transform.forward = deltaPosition;
        // }

        // Time penalty
        AddReward(-0.05f);

        // Fell off platform
        //if (this.transform.position.y < -1.0)
        //{
        //    Done();
        //    AddReward(-1.0f);
        //}
        previousDistance = distanceToTarget;

        // Actions, size = 2
	// punish turning
	//AddReward(vectorAction[1]);
        //HandleMovement(vectorAction);

	// Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
        Debug.Log($"Action X:{controlSignal.x}, theta:{controlSignal.y}");

        //rBody.AddForce(controlSignal * speed);
        rBody.transform.Rotate(3f*(controlSignal.x)*Vector3.up);
        Vector3 v = rBody.velocity;
        Vector3 rotated_v = Quaternion.AngleAxis(3f*(controlSignal.x), Vector3.up) * v;
        rBody.velocity = rotated_v;

        rBody.AddForce(20.0f * controlSignal.z*rBody.transform.forward);
        // rBody.AddForce(speed * rBody.transform.forward);

        this.steps = this.steps + 1;

        MoveCamera();

	      if (this.steps == this.maxStepsPerEpoch)
	      {
		        this.steps = 0;
		        Done();
	      }
    }

    private void HandleMovement(float[] action) {
		Vector3 dirToGo = Vector3.zero;
		Vector3 rotateDir = Vector3.zero;

		if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
		{
			dirToGo = transform.forward * Mathf.Clamp(action[0], -1f, 1f);
			rotateDir = transform.up * Mathf.Clamp(action[1], -1f, 1f);
		}
		else
		{
			switch ((int)(action[0]))
			{
				case 1:
					dirToGo = -transform.forward;
					break;
				case 2:
					rotateDir = -transform.up;
					break;
				case 3:
					rotateDir = transform.up;
					break;
			}
		}
		agentRigidbody.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
		transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

		if (agentRigidbody.velocity.sqrMagnitude > 25f) // slow it down
		{
			agentRigidbody.velocity *= 0.95f;
		}


    }
}