using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBounds : MonoBehaviour
{
    public float extremeBound = 500;
    public Terrain terrain;

    // Start is called before the first frame update
    void Start()
    {
        if(terrain == null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh.bounds = new Bounds(transform.position, Vector3.one * extremeBound);
        }
    }
}
