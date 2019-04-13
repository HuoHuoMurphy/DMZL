using Common;
using UnityEngine;
using UnityEngine.UI;

namespace DMZL_UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UI_Tips:MonoBehaviour
    {
        public Text TipsText;
        CanvasGroup group;
        float alpha;
        private void Awake ()
        {
            TipsText = transform.FindChildByName("TipsText").GetComponent<Text>();
            //获取Group
            group = GetComponent<CanvasGroup>();
            //获取透明度
            alpha = group.alpha;
            //隐藏窗口
            HideTips();
        }

        /// <summary>
        /// 显示Tips
        /// </summary>
        public void ShowTips ()
        {
            group.alpha = alpha;
            group.blocksRaycasts = true;
        }

        /// <summary>
        /// 显示Tips
        /// </summary>
        public void ShowTips (string text)
        {
            TipsText.text = text;
            ShowTips();
        }

        /// <summary>
        /// 显示Tips
        /// </summary>
        public void ShowTips ( string TextTable,string TextTag)
        {
            TipsText.text = LanguageManager.I.LoadWard(TextTable, TextTag);
            ShowTips();
        }

        /// <summary>
        /// 隐藏Tips
        /// </summary>
        public void HideTips ()
        {
            group.alpha = 0;
            group.blocksRaycasts = false;
        }
    }
}
