using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessAnimations : MonoBehaviour
{
    DepthOfField dof;
    Bloom bloom;
    PostProcessVolume volume;

    static PostProcessAnimations instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out dof);
        volume.profile.TryGetSettings(out bloom);
    }

    public static void StartFade()
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.Fade(lerpSpeed: 0.8f));
    }

    public static void StartReverseFade()
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.Fade(0.8f, 70, 300, 1.5f, 5f));
    }

    public IEnumerator Fade(float lerpSpeed = 1, float startValue = 300, float finalValue = 70, float bloomStartValue = 5f, float bloomEndValue = 1.5f)
    {
        instance.dof.focalLength.value = startValue;
        instance.bloom.intensity.value = bloomStartValue;

        while((Mathf.Round(instance.dof.focalLength.value/finalValue * 100)/100) != 1)
        {
            instance.dof.focalLength.value = Mathf.Lerp(instance.dof.focalLength.value, finalValue, lerpSpeed * Time.deltaTime);
            instance.bloom.intensity.value = Mathf.Lerp(instance.bloom.intensity.value, bloomEndValue, lerpSpeed * Time.deltaTime);

            yield return null;
        }

        instance.dof.focalLength.value = finalValue;
        instance.bloom.intensity.value = bloomEndValue;
        Debug.Log("Faded");
    }
}
