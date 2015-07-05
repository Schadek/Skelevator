using UnityEngine;
using System.Collections;
[ExecuteInEditMode]

[RequireComponent(typeof(Collider))]
public class ChangeFogColor : MonoBehaviour 
{
    public Color newFogColor;
    public float seconds;
    [Space(10)]
    public ChangeFogColor[] allFogChangers;


    private void OnTriggerEnter(Collider other)
    {
        foreach (ChangeFogColor i in allFogChangers)
        {
            i.StopAllCoroutines();
        }

        StartCoroutine(LerpFog());
    }

    private IEnumerator LerpFog()
    {
        float counter = 0f;

        while (counter < 1f)
        {
            counter += (float)Time.deltaTime / seconds;
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, newFogColor, counter);
            yield return null;
        }
    }
}
