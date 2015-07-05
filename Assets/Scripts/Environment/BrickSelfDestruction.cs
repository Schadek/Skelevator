using UnityEngine;
using System.Collections;

public class BrickSelfDestruction : MonoBehaviour 
{
    [SerializeField]
    private float delay = 3f;

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(FadeOutMaterial());
        Destroy(gameObject);
    }

    private IEnumerator FadeOutMaterial()
    {
        MeshRenderer mRend = GetComponent<MeshRenderer>();
        Material brickMat = mRend.material;
        Color newColor = Color.white;

        while (newColor.a > 0f)
        {
            newColor.a -= Time.deltaTime;
            brickMat.color = newColor;
            mRend.material = brickMat;
            yield return null;
        }
    }

    public void TriggerSelfDestruction()
    {
        StartCoroutine(SelfDestruct());
    }
}
