using UnityEngine;
using System.Collections;

public class DogAttributes : MonoBehaviour 
{
    public static DogAttributes Instance { get; set; }

    private void Start()
    {
        Instance = this;
    }

    public Skulls heldSkull = Skulls.None;
}
