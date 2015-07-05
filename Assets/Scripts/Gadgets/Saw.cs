using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//NOT USED, NOT MAINTAINED
public class Saw : Gadget
{

    public float range = 1f;

    //What happens if the player pressed the appropriate button and the IsUsable() function returned a true value
    public override bool Execute()
    {
        //PlayAnimation("SawStrike");
        //PlayAnimation("TargetDestroyed");
        ObjectDistance[] targets = GetTargets();

        if (targets.Length > 0)
            Destroy(targets[0].obj);

        return true;
    }
}
