using UnityEngine;
using System.Collections;

public class GoalObject : MonoBehaviour
{
    public float changeTime = 2f;
    public float lerpSpeed = 2f;
    public float maxMoveDistance = 10f;
    public float upperLimit, lowerLimit;

    [HideInInspector] public Transform bait;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, (upperLimit + lowerLimit) / 2, transform.position.z);
        InvokeRepeating("MoveGoal", 0, changeTime);
    }

    void MoveGoal()
    {
        StopAllCoroutines();

        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);
        var z = Random.Range(-1f, 1f);

        Vector3 newPos = Vector3.Normalize(new Vector3(x, y, z)) * maxMoveDistance;

        float height = transform.position.y + newPos.y;
        if (height > upperLimit || height < lowerLimit)
            newPos.y = 0;

        //transform.position = transform.position + newPos;

        StartCoroutine(MoveToPosition(transform.position + newPos));
    }

    IEnumerator MoveToPosition(Vector3 targetPos)
    {

        while (Vector3.Distance(transform.position, targetPos) > 0.5f)
        {
            if (bait == null)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, lerpSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }
}