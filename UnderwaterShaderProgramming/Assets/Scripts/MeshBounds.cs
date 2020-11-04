using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBounds : MonoBehaviour
{
    public float extremeBound = 500;

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh.bounds = new Bounds(transform.position, Vector3.one * extremeBound);
    }
}
