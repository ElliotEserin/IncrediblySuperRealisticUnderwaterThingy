using UnityEngine;

public class FishMotion : MonoBehaviour
{
    private Material fishMaterial;
    void Start()
    {
        fishMaterial = GetComponent<Renderer>().material;
        fishMaterial.SetFloat("_MoveOffset", Random.Range(0.0f, 3.14f));
    }
}
