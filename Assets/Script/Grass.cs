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
    private int _grassLevel;
    private bool _isActive = true;
    public DateTime UpdateAt { get; private set; }
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            _grassLevel = 0;

            childGrass.gameObject.SetActive(value);
            if(value)UpdateAt = DateTime.Now;
        }
    }
    public int grassLevel
    {
        get => _grassLevel;
        set
        {
            _grassLevel = value;
            if(value == 5)
            {
                SetGrassActive(1);
                GameManager.instance.GrassPoint++;
                var effect = Instantiate(levelUpEffect);
                effect.transform.position = transform.position;
            }
        }
    }
    public void SetGrassActive(int index)
    {
        for(int i = 0; i < grasses.Count; i++)
        {
            grasses[i].SetActive(i == index);
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
        SetGrassActive(0);
        UpdateAt = DateTime.Now;
        GameManager.instance.GrassPoint++;
        childGrass.eulerAngles = 
            new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
        childGrass.localPosition = 
            new Vector3(UnityEngine.Random.Range(
                -0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTouch()
    {
        for(int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                if (Mathf.Abs(i) == 2 && Mathf.Abs(j) == 2) continue;
                var target = GrassManager.instance.grasss.Find(g => g.Position == Position + new Vector2(i,j));
                if(target == null)
                {
                    var draft = GrassManager.instance.grasss.Find(g => !g.IsActive);
                    if(draft != null)
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
                else
                {
                    target.GetComponent<Grass>().grassLevel++;
                }
            }
        }
        UpdateAt = DateTime.Now;
    }
    public void Kill()
    {
        IsActive = false;
        Position = new Vector2(999, 999);
    }
}
