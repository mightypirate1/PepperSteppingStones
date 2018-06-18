using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperAgent : PepperAgentCommmon {

  // Like a normal PepperAgent, but it has a simplified motion-model!

  protected override void Move(float[] action, string t)
  {
    // Actions, size = 2
    Vector3 controlSignal = Vector3.zero;
    controlSignal.x = Mathf.Clamp(action[0], -1, 1);
    controlSignal.z = Mathf.Clamp(action[1], -1, 1);
    Debug.Log($"Action X:{controlSignal.x}, theta:{controlSignal.y}");

    rBody.AddForce(speed * controlSignal);
    rBody.transform.forward = Target.transform.position - rBody.transform.position;
    this.steps = this.steps + 1;
    MoveCamera();
  }
}
