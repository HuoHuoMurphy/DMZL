using UnityEngine;

namespace Common
{
    /// <summary>
    /// Boss
    /// </summary>
    public class GameBoss :MonoBehaviour
    {
        static GameObject g;
        public static GameObject G
        {
            get
            {
                if ( g == null )
                {
                    g = FindObjectOfType<GameBoss>().gameObject;
                    if ( g == null )
                    {
                        g = new GameObject("GameBoss");
                    }
                }
                    return g;
                }
        }

        public Canvas BossCanvas;
        private void Awake ()

        {
            DontDestroyOnLoad(this);
            if ( g == null )
            {
                g = gameObject;
                BossCanvas = GetComponentInChildren<Canvas>();
                BossCanvas.transform.SetAsFirstSibling();
            }       
        }

        /// <summary>
        /// 获取管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetManager<T> () where T : Component
        {
            return G.GetComponent<T>();
        }

        /// <summary>
        /// 增加管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T AddManager<T> () where T : Component
        {
            return G.AddComponent<T>();
        }
    }
}
