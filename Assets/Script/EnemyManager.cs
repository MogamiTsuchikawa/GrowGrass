using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private float sheepSpawnIntervalTime;
    [SerializeField] private List<OzisanPrehub> prehubs = new();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SheepSpawnLoop());
    }
    IEnumerator SheepSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(sheepSpawnIntervalTime);
            var lastUpdateGrass = GrassManager.instance.grasss
                .Where(
                    g => g.IsActive)
                .OrderByDescending(
                    g=>g.UpdateAt)
                .FirstOrDefault();
            var sheep = Instantiate(sheepPrefab);
            sheep.transform.position = lastUpdateGrass.transform.position 
                + new Vector3(
                    UnityEngine.Random.Range(-6f, 6f),
                    0.3f,
                    UnityEngine.Random.Range(-6f, 6f));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
