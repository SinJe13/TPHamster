using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class Maingame : MonoBehaviour
{
    public InputField InputField;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Space"))
        {
            Debug.Log(InputField);
        }
    }
}
