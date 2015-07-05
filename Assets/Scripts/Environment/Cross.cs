using UnityEngine;
using System.Collections;

public class Cross : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget;
    [Space(15)]
    public Quaternion upright;
    public Quaternion turned;

    public Color uprightColor;
    public Color turnedColor;
    public Color uprightParticle;
    public Color turnedParticle;

    public ParticleSystem pSystem;

    public bool isTurned;
    public Light crossLight;
    private MeshRenderer crossRend;
    private Material crossMat;

    private void Start()
    {
        if (!crossLight)
        {
            GetComponentInChildren<Light>();
        }
        if (!pSystem)
        {
            GetComponentInChildren<ParticleSystem>();
        }

        crossRend = GetComponent<MeshRenderer>();
        crossMat = crossRend.material;
        StartCoroutine(turnUpright());
    }

    public bool Execute(GadgetIdentifier usedGadget)
    {
        if (usedGadget == keyGadget)
        {
            StopAllCoroutines();
            if (isTurned)
            {
                StartCoroutine(turnUpright());
            }
            else
            {
                StartCoroutine(turnUpsideDown());
            }
            return true;
        }
        return false;
    }

    IEnumerator turnUpsideDown()
    {
        float counter = 0;
        isTurned = true;
        pSystem.startColor = uprightParticle;

        while (counter < 1)
        {
            counter += Time.deltaTime * 0.5f;
            transform.rotation = Quaternion.Slerp(transform.rotation, turned, counter);
            crossLight.color = Color.Lerp(crossLight.color, turnedColor, counter);
            crossMat.color = Color.Lerp(crossMat.color, Color.black, counter);
            crossRend.material = crossMat;
            yield return null;
        }
    } 

    IEnumerator turnUpright()
    {
        float counter = 0;
        isTurned = false;
        pSystem.startColor = turnedParticle;

        while (counter < 1)
        {
            counter += Time.deltaTime * 0.5f;
            transform.rotation = Quaternion.Slerp(transform.rotation, upright, counter);
            crossLight.color = Color.Lerp(crossLight.color, uprightColor, counter);
            crossMat.color = Color.Lerp(crossMat.color, Color.white, counter);
            crossRend.material = crossMat;
            yield return null;
        }
    }
}
