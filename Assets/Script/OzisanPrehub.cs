using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OzisanPrehub : MonoBehaviour
{
    [SerializeField] GameObject kusakariOzisanPrefab;
    [SerializeField] float spawnIntervalTime;
    // Start is called before the first frame update
    
    public void GenerateOzisan()
    {
        var ozi = Instantiate(kusakariOzisanPrefab);
        ozi.transform.position = transform.position;
        ozi.transform.LookAt(new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z));
    }
}
