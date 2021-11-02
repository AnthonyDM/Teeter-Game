using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHole : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            MainManager.Instance.NextLevel();
        }
    }
}
