using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float DespawnTime = 10f;
    public float speed = 8f;

    private float DespawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        DespawnTimer = DespawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        ///////// Bullet Movement //////////
        transform.position += transform.forward * speed * Time.deltaTime;

        /////////// DESPAWNS BULLETS //////////
        DespawnTimer -= Time.deltaTime;
        if(DespawnTimer <=0f)
        {
            Destroy (gameObject);
        }
    }
}
