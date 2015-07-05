using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AwakenPlayer : MonoBehaviour
{
    public Transform newDogPosition;
    public GameObject humanCharacter;
    public GameObject coffinModel;

    private void Start()
    {
        humanCharacter.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Dog") && DogAttributes.Instance.heldSkull == Skulls.Jacques) || (other.CompareTag("Dog") && GameInstance.GetInstance().debugMode))
        {
            //Dog is certainly wearing a skull
            StartCoroutine(ShortFade());
        }
    }

    private IEnumerator ShortFade()
    {
        Image fadeTexture = GameInstance.GetInstance().UI_Components.fadeTexture;
        Color newColor = fadeTexture.color;

        while (newColor.a < 1f)
        {
            newColor.a += Time.deltaTime;
            fadeTexture.color = newColor;
            yield return null;
        }

        MainCharacterAttributes.Instance.heldSkull = Skulls.Jacques;
        DogAttributes.Instance.heldSkull = Skulls.None;
        humanCharacter.SetActive(true);
        Destroy(coffinModel);
        GameInstance.GetInstance().Player_Components.dog.position = newDogPosition.position;

        yield return new WaitForSeconds(0.7f);

        while (newColor.a > 0f)
        {
            newColor.a -= Time.deltaTime;
            fadeTexture.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}
