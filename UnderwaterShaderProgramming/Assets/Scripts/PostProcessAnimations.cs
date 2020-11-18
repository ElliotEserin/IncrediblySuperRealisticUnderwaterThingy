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

        StartCoroutine(fade(lerpSpeed : 0.8f));
    }

    public static IEnumerator fade(float lerpSpeed = 1, float startValue = 300, float finalValue = 70)
    {
        instance.dof.focalLength.value = startValue;
        instance.bloom.intensity.value = 5f;

        while(instance.dof.focalLength.value / finalValue > 1.1f)
        {
            instance.dof.focalLength.value = Mathf.Lerp(instance.dof.focalLength.value, finalValue, lerpSpeed * Time.deltaTime);
            instance.bloom.intensity.value = Mathf.Lerp(instance.bloom.intensity.value, 1.5f, lerpSpeed * Time.deltaTime);
            yield return null;
        }

        instance.dof.focalLength.value = finalValue;
        instance.bloom.intensity.value = 1.5f;
        Debug.Log("Faded");
    }
}
