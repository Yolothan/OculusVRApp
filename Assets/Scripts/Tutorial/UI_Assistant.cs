using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Assistant : MonoBehaviour
{    
    private TextMeshProUGUI messageText;
    public string message;
    

    private void Awake()
    {
        messageText = transform.Find("message").Find("messageText").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    
    public void WriteText(string message)
    {
        TextWriter.AddWriter_Static(messageText, message, 0.05f, true, true);
    }

    
  
}
