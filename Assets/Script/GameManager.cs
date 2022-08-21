using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] TextMeshProUGUI grassPointText;
    private int _grassPoint = 0;
    [SerializeField] int countTime = 180;
    [SerializeField] TextMeshProUGUI countTimeText;
    [SerializeField] Canvas mainCanvas;
    public int GrassPoint
    {
        get => _grassPoint;
        set
        {
            _grassPoint = value;
            grassPointText.text = $"{value}pt";
        }
    }
    public enum Status
    {
        BeforePlay,Playing,AfterPlay
    }
    public Status GameStatus { get; private set; }= Status.Playing;
    public bool IsPlaying { get => GameStatus == Status.Playing; }
    

    IEnumerator CountDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (countTime == 0) break;
            countTime--;
            countTimeText.text = countTime.ToString();
        }
        GameStatus = Status.AfterPlay;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
        mainCanvas.gameObject.SetActive(true);
        
    }
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying) return;
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
