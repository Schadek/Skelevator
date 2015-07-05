using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Light))]
public class Torch : MonoBehaviour, IInteractable
{
    public GadgetIdentifier keyGadget;
    public Light lightSource;
    public ParticleEmitter[] pEmitter;
    [Space(15)]
    public bool lit;

    /*********************************************************/
    //The maximum intensity the torch has when lit
    private float maxIntensity;
    //The maximum/minimum size each particle can reach when activated
    private float[] maxSizeEmitter;
    private float[] minSizeEmitter;
    /*********************************************************/

    private InteractableEntries entries;
    public InteractableEntries Entries
    {
        get
        {
            if (!entries)
            {
                entries = GetComponent<InteractableEntries>();
            }
            return entries;
        }
    }

    private void Start()
    {
        maxIntensity = lightSource.intensity;

        maxSizeEmitter = new float[pEmitter.Length];
        minSizeEmitter = new float[pEmitter.Length];

        for (int i = 0; i < pEmitter.Length; i++)
        {
            maxSizeEmitter[i] = pEmitter[i].maxSize;
            minSizeEmitter[i] = pEmitter[i].minSize;
        }

        //If the torch is marked as extinguished we do so at the start of the game
        if (!lit)
        {
            lightSource.intensity = 0;
            StartCoroutine(PutOutTorch());
        }
    }

    public bool Execute(GadgetIdentifier ident)
    {
        if (gameObject.activeSelf)
        {
            if (ident == keyGadget)
            {
                StopAllCoroutines();
                if (lit)
                {
                    StartCoroutine(PutOutTorch());
                }
                else 
                {
                    StartCoroutine(LightTorch());
                }
                return true;
            }
        }
        return false;
    }

    private IEnumerator PutOutTorch()
    {
        float counter = 0f;
        lit = false;

        foreach (ParticleEmitter i in pEmitter)
        {
            i.maxSize = 0f;
            i.minSize = 0f;
        }

        while (counter < 1f)
        {
            counter += Time.deltaTime * 0.5f;
            lightSource.intensity = Mathf.SmoothStep(lightSource.intensity, 0, counter);

            yield return null;
        }
    }

    private IEnumerator LightTorch()
    {
        float counter = 0f;
        lit = true;

        for (int i = 0; i < pEmitter.Length; i++)
        {
            pEmitter[i].maxSize = maxSizeEmitter[i];
            pEmitter[i].minSize = minSizeEmitter[i];
        }

        while (counter < 1f)
        {
            counter += Time.deltaTime * 0.5f;
            lightSource.intensity = Mathf.SmoothStep(lightSource.intensity, maxIntensity, counter);

            yield return null;
        }
    }
}
