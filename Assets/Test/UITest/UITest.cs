using System.Collections;
using System.Collections.Generic;
using Common;
using DMZL_UI;
using UnityEngine;

public class UITest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.I.GetWindow<UI_SystemButton>().Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
