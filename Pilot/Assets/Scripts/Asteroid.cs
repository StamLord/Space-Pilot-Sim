using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Metal, IDamagable
{
    public int health = 100;
    public Vector2 childSpawn = new Vector2(3, 9);
    public GameObject childPrefab;

    public void Damage(int damage)
    {
        health -= damage;
        if(health <= 0)
            Explode();
    }

    void Explode()
    {
        float children = Random.Range(childSpawn.x, childSpawn.y);

        for (int i = 0; i < children; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * transform.localScale.x + transform.position;
            Vector3 randomAngle = Random.insideUnitSphere * 360;
            Vector3 scale = transform.localScale / children;

            GameObject child = GameObject.Instantiate(childPrefab, randomPosition, Quaternion.Euler(randomAngle)) as GameObject;
            Asteroid asteroid = child.GetComponent<Asteroid>();
            asteroid.metalContent = metalContent / children;
            
            child.transform.localScale = scale;
        }

        Destroy(gameObject);
    }
}
