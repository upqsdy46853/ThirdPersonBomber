using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button button;
    public TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // PlayerPrefs.SetString("PlayerNickname", inputField.text);
        // PlayerPrefs.Save();
    }

    public void onJoinGameClicked()
    {
        PlayerPrefs.SetString("PlayerNickname", inputField.text);
        PlayerPrefs.Save();
        button.interactable = false;
        buttonText.text = "Joining...";
    }

}
