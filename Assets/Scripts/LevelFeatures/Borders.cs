using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{
    private SpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(SpawnBallAfterDieCorroutine());
        }
    }

    IEnumerator SpawnBallAfterDieCorroutine()
    {
        yield return new WaitForSeconds(1f);
        spawnManager.SpawnBall();

    }
}

