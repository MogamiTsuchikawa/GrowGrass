using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> kusaResultTexts = new();
    private List<string> kusaKinds = new() { "‘Lv1", "‘Lv2", "‘Lv3" };
    private List<int> kusaMinLevel = new() { 0, 2, 6 };
    private List<int> kusaLevelPoint = new() { 10, 50, 100 };
    [SerializeField] TextMeshProUGUI kusaPointResultText, kusaResultText;
    [SerializeField] GameObject backToTitleBtn, replayBtn;
    int totalPoint = 0;
    public void StartResultView()
    {
        StartCoroutine(ResultCountUp(0));
    }
    void OnEndResultCountUp()
    {
        kusaPointResultText.text = $"+{GameManager.instance.GrassPoint}pt";
        totalPoint += GameManager.instance.GrassPoint;
        kusaResultText.text = totalPoint.ToString();
        backToTitleBtn.SetActive(true);
        replayBtn.SetActive(true);
    }
    private void Start()
    {
        backToTitleBtn.SetActive(false);
        replayBtn.SetActive(false);
    }
    IEnumerator ResultCountUp(int targetIndex)
    {
        int result = GrassManager.instance.grasss.Count(
            g =>
                kusaMinLevel[targetIndex] <= g.GrassLevel
                && g.GrassLevel <= (
                    targetIndex == 2
                    ? 20
                    : kusaMinLevel[targetIndex + 1]
                )
        );
        for (int i=0;i<100;i++)
        {
            int p = kusaLevelPoint[targetIndex] * (result * i / 100);
            kusaResultTexts[targetIndex].text = $"{kusaKinds[targetIndex]}~{kusaLevelPoint[targetIndex]}={p}";
            yield return new WaitForSeconds(0.01f);
        }
        kusaResultTexts[targetIndex].text = $"{kusaKinds[targetIndex]}~{kusaLevelPoint[targetIndex]}={result * kusaLevelPoint[targetIndex]}";
        totalPoint += result * kusaLevelPoint[targetIndex];
        if(targetIndex == 2)
        {
            OnEndResultCountUp();
            yield return null;
        }
        StartCoroutine(ResultCountUp(targetIndex+1));
    }
    public void OnClickBackToTitleBtn()
    {

    }
    public void OnClickReplayBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }
}
