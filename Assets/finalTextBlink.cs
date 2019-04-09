using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class finalTextBlink : MonoBehaviour
{
    float blinkTime = 0.33f;
    float counter = 0;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= blinkTime)
        {
            counter -= blinkTime;
            text.enabled = !text.enabled;
        }
    }
}
