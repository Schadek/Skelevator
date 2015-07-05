using UnityEngine;
using System.Collections;

public class KnockDoorOff : MonoBehaviour, IInteractable
{
    public Door door;
    public GameObject handle;
    public bool mustBeOpen;
    public GadgetIdentifier keyGagdet;

    public bool Execute(GadgetIdentifier usedGadget)
    {
        if (usedGadget == keyGagdet)
        {
            if (mustBeOpen)
            {
                if (door.open)
                {
                    KnockOff();
                    return true;
                }
            }
            else
            {
                KnockOff();
                return true;
            }
        }
        return false;
    }

    private void KnockOff()
    {
        //We add a rigidbody
        Rigidbody rBody = door.gameObject.AddComponent<Rigidbody>();
        //To add a bit of juicyness to it we add a velocity
        rBody.AddForce((transform.position - GameInstance.GetInstance().Player_Components.human.position).normalized * 10f, ForceMode.VelocityChange);

        //Although this is a bit hacky, we remove the ability to be interacted with by changing the door's layer
        handle.layer = 0;
    }
}
