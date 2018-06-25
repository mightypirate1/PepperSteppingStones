using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperAgentFat : PepperAgentCommmon{

  public PepperAgentFat()
  {
    //Fat agent is so fat it needs to redefine this variable for the win condition to work!
    this.targetDistance = 2.0f;
  }

  //Fat agent uses the same motion model as the simplified one...
  protected override void Move(float[] action, string t)
  {
    // Actions, size = 2
    Vector3 controlSignal = Vector3.zero;
    controlSignal.x = Mathf.Clamp(action[0], -1, 1);
    controlSignal.y = Mathf.Clamp(0.0f,-1,1);
    controlSignal.z = Mathf.Clamp(action[1], -1, 1);

    float x = 1.0f / Mathf.Max( Mathf.Abs(controlSignal.x), Mathf.Abs(controlSignal.z));
    Vector3 tmp = x * controlSignal;
    controlSignal = controlSignal / tmp.magnitude;

    // Debug.Log($"Action X:{controlSignal.x}, theta:{controlSignal.z}");

    rBody.AddForce(10f * controlSignal);
    rBody.transform.forward = Target.transform.position - rBody.transform.position;
    this.steps = this.steps + 1;
    MoveCamera();
  }
}
