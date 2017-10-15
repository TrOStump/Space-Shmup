using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4 : Enemy
{
    // Enemy_4 will start offscreen and then pick a random point on screen to
    //   move to. Once it has arrived, it will pick another random point and
    //   continue until the player has shot it down.

    public Vector3[] points;  // Stores the p0 & p1 for interpolation
    public float timeStart;  // Birth time for this Enemy_4
    public float duration = 4;  // Duration of movement

    void Start()
    {
        points = new Vector3[2];
        // There is already an initial position chosen by Main.SpawnEnemy()
        //   so add it to points as the initial p0 & p1
        points[0] = pos;
        points[1] = pos;

        InitMovement();
    }

    void InitMovement()
    {
        // Pick a new point to move to that is on screen
        Vector3 p1 = Vector3.zero;
        float esp = Main.S.enemySpawnPadding;
        Bounds cBounds = Utils.camBounds;
        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

        points[0] = points[1];  // Shift points[1] to points[0]
        points[1] = p1;         // Add p1 as points[1]

        // Reset the time
        timeStart = Time.time;
    }

    public override void Move()
    {
        // This completely overrides Enemy.Move() with a linear interpolation

        float u = (Time.time - timeStart) / duration;
        if (u >= 1)
        {  // if u >=1...
            InitMovement();  // ...then initialize movement to a new point
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);         // Apply Ease Out easing to u

        pos = (1 - u) * points[0] + u * points[1]; // Simple linear interpolation
    }
}
