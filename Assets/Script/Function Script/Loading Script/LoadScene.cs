using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public Text tips;
    public Image image;
    public GameObject imagewrapper;
    AsyncOperation loadingOperation;
    

    float object_width;

    void Start()
    {
        List<string> words = new List<string>();
        words.Add("Hidup seperti larry!");
        words.Add("Hadehh mumet da mumet!");
        words.Add("Gak ada tips dan gak tau mau nulis apa hehe");

        int tmp_rng = Random.Range(0, words.Count);

        tips.text = words[tmp_rng];

        object_width = 0f;
        
        loadingOperation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
    }
    
    void Update()
    {
        float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        float tmp = Mathf.Round(progressValue * 100);

        object_width += ((250f/100f) * tmp);
        image.rectTransform.sizeDelta = new Vector2(object_width,imagewrapper.GetComponent<RectTransform>().rect.height);
        image.rectTransform.localPosition = new Vector3(-(imagewrapper.GetComponent<RectTransform>().rect.width/2) + (object_width/2),0,0);
    }
}
