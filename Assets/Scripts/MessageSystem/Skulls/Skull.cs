using UnityEngine;
using System.Collections;

public class Skull : MonoBehaviour 
{
    public Skulls skeletonID;
    protected MessageSystem messenger;

    private void Start()
    {
        messenger = MessageSystem.GetInstance();
    }

    public virtual void OnPickUp()
    {

    }
}
