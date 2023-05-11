using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    Light2D Light;

    float size;
    float max;
    [SerializeField]float min = 0.08f;
    [SerializeField]float maxT;
    [SerializeField]float minT;
    [SerializeField]bool flickIntensity = true;

    void Start()
    {
        Light = gameObject.GetComponent<Light2D>();
        max = Light.intensity;
        size = Light.pointLightOuterRadius;
        StartCoroutine(FlickIntensity());
    }

    private IEnumerator FlickIntensity()
    {
        float t0 = Time.time;
        float t = t0;
        WaitUntil wait = new WaitUntil(() => Time.time > t0 + t);
        yield return new WaitForSeconds(Random.Range(0.01f, 0.5f));

        while (true)
        {
            if (flickIntensity)
            {
                t0 = Time.time;
                float r = Random.Range(min, max);
                Light.intensity = r;
                Light.pointLightOuterRadius = size + r;
                t = Random.Range(minT, maxT);
                yield return wait;
            }
            else yield return null;
        }
    }
}
