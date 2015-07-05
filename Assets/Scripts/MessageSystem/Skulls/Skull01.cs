using UnityEngine;
using System.Collections;

public class Skull01 : Skull
{
    [Space(10)]
    [TextArea(3, 15)]
    public string firstPickUpDialogue;
    [TextArea(3, 15)]
    [Space(10)]
    public string pickUpDialogue;
    [Space(10)]
    public GameObject godRay;

    private bool firstTimePickedUp = true;

    public override void OnPickUp()
    {
        if (firstTimePickedUp)
        {
            messenger.StartDialogue(firstPickUpDialogue);
            //godRay.GetComponent<FadingLight>().FadeOut(3f);
        }
        else
        {
            messenger.StartDialogue(pickUpDialogue);
        }

        Destroy(this.gameObject);
    }
}
