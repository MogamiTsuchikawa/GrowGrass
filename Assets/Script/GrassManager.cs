using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    public static GrassManager instance;

    public List<Grass> grasss = new();
    public float naturalSpreadIntervalTime;
    public float autoLevelUpIntervalTime;
    public GameObject grassPrefab;
    public bool hasAutoLevelUpSkill = false;
    public bool hasNaturalSpreadSkill = false;
    public bool hasSpreadSeedSkill = false;
    // Start is called before the first frame update
    void Start()
    {
        
        //for(int i = 0; i < 5000; i++)
        //{
        //    var g = Instantiate(grassPrefab);
        //    g.GetComponent<Grass>().IsActive = false;
        //    grasss.Add(g.GetComponent<Grass>());
        //}
    }
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
