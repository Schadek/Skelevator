using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour, IInteractable
{
    public GameObject brickParent;
    public GadgetIdentifier keyGagdet;
    private Rigidbody[] bricks;
    private BrickSelfDestruction[] destructors;


    private void Start()
    {
        bricks = brickParent.GetComponentsInChildren<Rigidbody>();
        destructors = brickParent.GetComponentsInChildren<BrickSelfDestruction>();
        brickParent.SetActive(false);
    }

    public bool Execute(GadgetIdentifier usedGadget)
    {
        if (usedGadget == keyGagdet)
        {
            brickParent.SetActive(true);
            Vector3 forceVector = new Vector3(0, 0, -10f);
            for (int i = 0; i < bricks.Length; i++)
            {
                bricks[i].AddForce(forceVector, ForceMode.VelocityChange);
                destructors[i].TriggerSelfDestruction();
            }
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
