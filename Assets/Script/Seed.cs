using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Seed : MonoBehaviour
{
    private Rigidbody rigidbody;
    [SerializeField] GameObject grassPrefab;
    [SerializeField] float power;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Vector3 force = Vector3.up * 5
            + Vector3.forward * UnityEngine.Random.Range(-3f, 3f) 
            + Vector3.right * UnityEngine.Random.Range(-3f,3f);
        rigidbody.AddForce(force * power);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 0)
        {
            int x = Mathf.RoundToInt(transform.position.x);
            int z = Mathf.RoundToInt(transform.position.z);
            if(GrassManager.instance.grasss.Count(g=>g.Position == new Vector2(x, z)) == 0)
            {
                var g = Instantiate(grassPrefab);
                g.transform.parent = GrassManager.instance.transform;
                g.GetComponent<Grass>().Position = new Vector2(x, z);
            }
            Destroy(gameObject);
        }
    }
}
