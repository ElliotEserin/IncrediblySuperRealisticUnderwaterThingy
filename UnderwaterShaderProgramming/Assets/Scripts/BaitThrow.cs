using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitThrow : MonoBehaviour
{
    public GameObject baitPrefab;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            var bait = Instantiate(baitPrefab, transform.position, Quaternion.identity);
            Destroy(bait, 10);
        }
    }
}
