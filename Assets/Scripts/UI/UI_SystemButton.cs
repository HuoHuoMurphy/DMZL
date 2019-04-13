using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Common;
using DMZL_UI;

public class UI_SystemButton :UIWindow
{
    Button button;
    UI_SystemWindow systemWindow;
    Text text;
    public override void Initialize ()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }
    private void Start ()
    {
        //获取 UI_SystemWindow
        systemWindow = UIManager.I.GetWindow<UI_SystemWindow>();
        //文字将注册进入LanguageManager
        LanguageManager.I.TextRegister("WordTable_Test","SystemWindow",text);
        //给按钮附开关功能
        button.onClick.AddListener(systemWindowIO);
        //再Update中检测是否按ESC
        UpdateManager.I.OnSystemUpdate(systemWindowESCIO);
    }
    void systemWindowIO ()
    {    
        //如果系统窗口关闭则打开，窗口打开则关闭
        if ( !systemWindow.IO )
        {
            systemWindow.Show();
        }
        else
        {
            systemWindow.Hide();
        }
    }
    void systemWindowESCIO ()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            systemWindowIO();
        }
    }

}
