using UnityEngine;
using System.Collections;

public class DogTrigger : MonoBehaviour
{
    public MeshRenderer mRend;
    public Color outlineColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            Debug.Log("is dog");
            Material mainMaterial = mRend.material;
            mainMaterial.shader = Shader.Find("Standard (Outlined)");
            mainMaterial.SetColor("_OutlineColor", outlineColor);
            mainMaterial.SetFloat("_Outline", 0.012f);
            mRend.material = mainMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            Material mainMaterial = mRend.material;
            mainMaterial.shader = Shader.Find("Standard");
            mRend.material = mainMaterial;
        }
    }
}
