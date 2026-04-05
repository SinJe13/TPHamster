using System.Collections;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public IEnumerator Show(GameObject panel)
    {
        panel.SetActive(true);

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        RectTransform rt = panel.GetComponent<RectTransform>();

        cg.alpha = 0;
        rt.localScale = Vector3.one * 0.8f;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            cg.alpha = Mathf.Lerp(0, 1, t);
            rt.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);
            yield return null;
        }
    }

    public IEnumerator Hide(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        RectTransform rt = panel.GetComponent<RectTransform>();

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            cg.alpha = Mathf.Lerp(1, 0, t);
            rt.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.8f, t);
            yield return null;
        }

        panel.SetActive(false);
    }
}