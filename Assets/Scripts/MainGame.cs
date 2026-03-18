using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    private string pseudonyme = " ";

    void OnGUI()
    {
        pseudonyme = GUI.TextField(new Rect(435, 65, 115, 20), pseudonyme, 15);
    }

    public string getPseudo()
    {
        Debug.Log(pseudonyme); 
        return pseudonyme;
    }

    public void DisplayPseudo()
    {
        getPseudo();
    }
}
