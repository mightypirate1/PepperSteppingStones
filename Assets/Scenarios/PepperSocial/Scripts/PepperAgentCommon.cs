using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperAgentCommmon : Agent {

    public float ArenaDimensions = 20.0f;
    public float speed = 1f;
    public float rotation_speed = 0.3f;
    // What the agent is chasing
    public Transform Target;
    public GameObject cam;

    protected Rigidbody rBody;

    protected float previousDistance = float.MaxValue;

    protected Rigidbody agentRigidbody;
    protected Vector3 offset = new Vector3(0.0f,1.0f,0.0f);

    protected float moveSpeed;
    protected float turnSpeed;

    protected int maxStepsPerEpoch;
    protected int steps;

    virtual protected void MoveCamera()
    {

    }

    virtual protected void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void InitializeAgent()
    {

    }

    public override void CollectObservations()
    {

    }

    public override void AgentReset()
    {
        float allowedArea = ArenaDimensions * 0.4f;

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
	      this.maxStepsPerEpoch = 1000;
        this.previousDistance = 0f;
    }

    protected void RewardLinear(float d, float prev_d)
    {
            // Getting closer
            if (d < prev_d)
            {
                float delta_d = 0.1f*(prev_d - d);
                AddReward(delta_d);
            }
            else
            {
              // Time penalty
              AddReward(-0.025f);
            }
            // Time penalty
            AddReward(-0.025f);
    }

    protected void RewardConstant(float d, float prev_d)
    {
      // Getting closer
      if (d < prev_d)
        AddReward(0.1f);
      // Time penalty
      AddReward(-0.05f);
    }

    protected void RewardDirect(float d, float prev_d)
    {
      float delta_d = 0.1f*(prev_d - d);
      AddReward(delta_d - 0.05f);
    }

    protected void CheckReward()
    {
      // Rewards
      float distanceToTarget = Vector3.Distance(this.transform.position,
                                                Target.position);
      // RewardLinear(distanceToTarget,previousDistance);
      // RewardDirect(distanceToTarget,previousDistance);
      RewardConstant(distanceToTarget,previousDistance);

      this.previousDistance = distanceToTarget;

    }



    protected virtual void Move(float[] action, string text)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(action[0], -1, 1);
        controlSignal.z = Mathf.Clamp(action[1], -1, 1);
        Debug.Log($"Action X:{controlSignal.x}, theta:{controlSignal.y}");

        //rBody.AddForce(controlSignal * speed);
        rBody.transform.Rotate(3f*(controlSignal.x)*Vector3.up);
        Vector3 v = rBody.velocity;
        Vector3 rotated_v = Quaternion.AngleAxis(3f*(controlSignal.x), Vector3.up) * v;
        rBody.velocity = rotated_v;

        rBody.AddForce(10.0f * controlSignal.z*rBody.transform.forward);
        // rBody.AddForce(speed * rBody.transform.forward);

        this.steps = this.steps + 1;

        MoveCamera();
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Move(vectorAction,textAction);
        CheckReward();

        if (this.steps == this.maxStepsPerEpoch || Vector3.Distance(this.transform.position,
                                                  Target.position) < 0.5f)
        {
            this.steps = 0;
            Done();
        }
    }
}
