using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Grass : MonoBehaviour,ITouchable
{
    [SerializeField] Transform childGrass;
    [SerializeField] GameObject levelUpEffect;
    [SerializeField] List<GameObject> grasses;
    [SerializeField] GameObject grassPrefab;
    [SerializeField] GameObject seedPrefab;
    private int _grassLevel;
    private bool _isActive = true;
    private bool flowerable = false;
    public bool IsGrass { get; set; } = true;
    public DateTime UpdateAt { get; private set; }
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            _grassLevel = 0;

            childGrass.gameObject.SetActive(value);
            if (value)
            {
                UpdateAt = DateTime.Now;
                StartCoroutine(NaturalSpread());
                flowerable = GrassManager.instance.hasSpreadSeedSkill && UnityEngine.Random.Range(0, 10) > 8;
            }
        }
    }
    public int GrassLevel
    {
        get => _grassLevel;
        set
        {
            _grassLevel = value;
            if(value == 0)
            {
                GrassActiveIndex = 0;
            }
            if(value == 2)
            {
                GrassActiveIndex = 1;
                GameManager.instance.GrassPoint++;
                //var effect = Instantiate(levelUpEffect);
                //effect.transform.position = transform.position;
            }
            if(value == 6)
            {
                GrassActiveIndex = 2;
                GameManager.instance.GrassPoint++;
                var effect = Instantiate(levelUpEffect);
                effect.transform.position = transform.position;
            }
            if(value == 9 && flowerable)
            {
                GrassActiveIndex = 3;
                GameManager.instance.GrassPoint++;
            }
            if(value == 11 && flowerable)
            {
                GrassActiveIndex = 4;
                GameManager.instance.GrassPoint++;
            }
            if (value == 14 && flowerable)
            {
                GrassActiveIndex = 5;
                GameManager.instance.GrassPoint++;
            }
        }
    }
    private int _grassActiveIndex = 0;
    private int GrassActiveIndex
    {
        get => _grassActiveIndex;
        set
        {
            _grassActiveIndex = value;
            Debug.Log(value);
            for (int i = 0; i < grasses.Count; i++)
            {
                grasses[i].SetActive(i == value);
            }
        }
    }
    public Vector2 Position
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.z);
        }
        set
        {
            transform.position = new Vector3(value.x, 0, value.y);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GrassActiveIndex = 0;
        UpdateAt = DateTime.Now;
        GameManager.instance.GrassPoint++;
        childGrass.eulerAngles = 
            new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
        childGrass.localPosition = 
            new Vector3(UnityEngine.Random.Range(
                -0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f));
        StartCoroutine(NaturalSpread());
        flowerable = GrassManager.instance.hasSpreadSeedSkill && UnityEngine.Random.Range(0, 10) > 8;
    }
    IEnumerator AutoLevelUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                GrassManager.instance.autoLevelUpIntervalTime 
                    * UnityEngine.Random.Range(0.5f, 1.5f));
            if (!GrassManager.instance.hasAutoLevelUpSkill) continue;
            if (!GameManager.instance.IsPlaying) break;
            GrassLevel++;
            if (GrassLevel >= 20) break;
            
        }
    }
    IEnumerator NaturalSpread()
    {
        StartCoroutine(AutoLevelUp());
        while (true)
        {
            yield return new WaitForSeconds(
                GrassManager.instance.naturalSpreadIntervalTime 
                    * UnityEngine.Random.Range(0.5f,1.5f));
            if (!GrassManager.instance.hasNaturalSpreadSkill) continue;
            if (!GameManager.instance.IsPlaying) break;
            int count = 0;
            for(int i = -1; i < 2; i++)
                for(int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var grass = FindGrass(i, j);
                    if (grass == null)
                    {
                        SetNewGrass(i, j);
                        count++;
                    }
                }
            if (count == 0 || !IsActive) break;
            
        }
    }

    public void OnTouch()
    {
        for(int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                if (Mathf.Abs(i) == 2 && Mathf.Abs(j) == 2) continue;
                var target = GrassManager.instance.grasss.Find(g => g.Position == Position + new Vector2(i, j));
                if (target == null)
                {
                    SetNewGrass(i, j);
                }
                else
                {
                    target.GetComponent<Grass>().GrassLevel++;
                }
            }
        }
        UpdateAt = DateTime.Now;
        if(GrassActiveIndex == 5)
        {
            SpreadSeed();
        }
    }
    private void SpreadSeed()
    {
        GrassActiveIndex = 2;
        for(int i = 0; i < 8; i++)
        {
            var seed = Instantiate(seedPrefab);
            seed.transform.position = transform.position + Vector3.up * 0.5f;
        }
    }
    private void SetNewGrass(int i,int j)
    {
        var draft = GrassManager.instance.grasss.Find(g => !g.IsActive);
        if (draft != null)
        {
            draft.IsActive = true;
            draft.Position = Position + new Vector2(i, j);
        }
        else
        {
            var newGrass = Instantiate(grassPrefab);
            newGrass.transform.parent = GrassManager.instance.transform;
            newGrass.GetComponent<Grass>().Position = Position + new Vector2(i, j);
            GrassManager.instance.grasss.Add(newGrass.GetComponent<Grass>());
        }
    }
    Grass[,] cacheGrassArray = new Grass[3, 3];
    private Grass FindGrass(int i,int j)
    {
        if(cacheGrassArray[i+1,j+1] != null)
        {
            if(cacheGrassArray[i+1,j+1].Position == Position + new Vector2(i,j))
                return cacheGrassArray[i+1,j+1];
        }
        var g = GrassManager.instance.grasss.Find(g => g.Position == Position + new Vector2(i, j));
        cacheGrassArray[i + 1, j + 1] = g;
        return g;
    }
    public void Kill()
    {
        IsActive = false;
        Position = new Vector2(999, 999);
    }
}
