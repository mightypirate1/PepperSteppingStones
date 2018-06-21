using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperAgentSecond : PepperAgentCommmon{

    protected override void MoveCamera()
    {
        // Vector3 offset = new Vector3(0.0f,1.0f,0.0f);
        Vector3 offset = 1f*rBody.transform.up + 0.1f*rBody.transform.forward;
        this.cam.transform.position = transform.position + offset;
        this.cam.transform.forward = Quaternion.AngleAxis(40, rBody.transform.right) * rBody.transform.forward;
    }

    public override void InitializeAgent()
    {
      this.cam = GameObject.Find("Main Camera");
      //agentParameters.agentCameras.Add( cam.GetComponent<Camera>() );
    }

    public override void CollectObservations()
    {

    }
}
