﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hammer : Gadget
{
    //If the Gadget is invoked, this function gets executed. For further details about the structure of the gadget system, refer to IInteractable.cs
    public override bool Execute()
    {
        ObjectDistance[] containers = GetTargets();
        IInteractable[] targets;


        for (int i = 0; i < containers.Length; i++)
        {
            targets = containers[i].obj.GetComponents<IInteractable>();
            if (TryInvoking(targets, GadgetIdentifier.Hammer))
            {
                //If at least one IInteractable was successfully invoked, we do not need to go any further than this
                return true;
            }
        }
        return false;
    }
}
