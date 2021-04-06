﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class writeText : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gm;
    TMP_Text text;
    void Start()
    {
        gm = GameManager.GetInstance();
        text = GetComponent<TMP_Text>();
        if(gm.win)
        {
            text.text = "YOU WIN!";
        }
        else{
            text.text = "GAME OVER";
        }
    }
}
