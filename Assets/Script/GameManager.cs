using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] TextMeshProUGUI grassPointText;
    private int _grassPoint = 0;
    public int GrassPoint
    {
        get => _grassPoint;
        set
        {
            _grassPoint = value;
            grassPointText.text = $"ëê:{value}pt";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        TouchFunction();
    }
    void TouchFunction()
    {
        RaycastHit hit;
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    TouchedObjectFunction(hit.transform.gameObject);
                }
            }
            

        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            
        }
    }
    void TouchedObjectFunction(GameObject obj)
    {
        ITouchable touchable = obj.GetComponent<ITouchable>();
        if (touchable == null) return;
        touchable.OnTouch();
        
    }
}
interface ITouchable
{
    void OnTouch();
}
