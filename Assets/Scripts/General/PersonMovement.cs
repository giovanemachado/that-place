using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    float moveSpeed = 0.5f;

    float maxXMovement = 7.5f;
    float minXMovement = -7.5f;

    float maxYMovement = 0.5f;
    float minYMovement = -0.5f;

    float tChange = 0; // force new direction in the first Update
    float randomX;
    float randomY;

    private void Update()
    {
        Vector3 position = new Vector3(0, 0, transform.position.z);

        // change to random direction at random intervals
        if (Time.time >= tChange)
        {
            randomX = Random.Range(-2.0f, 2.0f); // with float parameters, a random float
            randomY = Random.Range(-2.0f, 2.0f); //  between -2.0 and 2.0 is returned
                                               // set a random interval between 0.5 and 1.5
            tChange = Time.time + Random.Range(0.5f, 1.5f);
        }

        transform.Translate(new Vector3(randomX, randomY, 0) * moveSpeed * Time.deltaTime);

        // if object reached any border, revert the appropriate direction
        if (transform.position.x >= maxXMovement || transform.position.x <= minXMovement)
        {
            randomX = -randomX;
        }
        if (transform.position.y >= maxYMovement || transform.position.y <= minYMovement)
        {
            randomY = -randomY;
        }

        // make sure the position is inside the borders
        position.x = Mathf.Clamp(transform.position.x, minXMovement, maxXMovement);
        position.y = Mathf.Clamp(transform.position.y, minYMovement, maxYMovement);

        transform.position = position;
    }
}
