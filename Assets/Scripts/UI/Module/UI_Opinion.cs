using Common;
using UnityEngine;
using UnityEngine.UI;

namespace DMZL_UI
{
    /// <summary>
    /// 选项脚本
    /// </summary>
    public class UI_Opinion :MonoBehaviour, IPooler
    {
        Text optionText;
        UI_Tips tips;
        Button button;

        private void Awake ()
        {
            optionText = transform.FindChildByName("OptionText").GetComponent<Text>();
            tips = transform.FindChildByName("Tips").GetComponent<UI_Tips>();
            button = transform.FindChildByName("Button").GetComponent<Button>();          
        }
        private void Start ()
        {
            //鼠标悬停3秒后显示Tips
            button.OnMouseStay(() => tips.ShowTips(),3);
            //鼠标离开隐藏Tips
            button.OnMouseExit(() => tips.HideTips());
        }
        /// <summary>
        /// 注入VO
        /// </summary>
        /// <param name="vo">选项VO</param>
        public void FitVO (VO_Opinion vo)
        {
            //语言系统注册选项Text
            LanguageManager.I.TextRegister(vo.TextTable,vo.OptionTag,optionText);
            //语言系统注册TipsText
            LanguageManager.I.TextRegister(vo.TextTable,vo.TipsTag,tips.TipsText);
            //按钮注册事件
            button.onClick.AddListener(vo.Action);
        }
        public void OnReset ()
        {
           
        }

        public void Recover ()
        {
            LanguageManager.I.RemoveText(optionText);
            LanguageManager.I.RemoveText(tips.TipsText);
            button.onClick.RemoveAllListeners();
        }
    }
}
