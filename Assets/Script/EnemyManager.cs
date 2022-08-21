using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private float sheepSpawnIntervalTime;
    [SerializeField] private float ozisanSpawnIntervalTime;
    [SerializeField] private List<OzisanPrehub> prehubs = new();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SheepSpawnLoop());
        StartCoroutine(OzisanSpawnLoop());
    }
    IEnumerator SheepSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(sheepSpawnIntervalTime);
            if (!GameManager.instance.IsPlaying) continue;
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
    IEnumerator OzisanSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(ozisanSpawnIntervalTime);
            if (!GameManager.instance.IsPlaying) continue;
            prehubs[UnityEngine.Random.Range(0, prehubs.Count)].GenerateOzisan();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
