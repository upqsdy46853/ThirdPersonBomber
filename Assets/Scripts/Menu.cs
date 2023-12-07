using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField inputField;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnJoinGameClicked(){
        PlayerPrefs.SetString("PlayerNickname", inputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SampleScene");
    }
}
