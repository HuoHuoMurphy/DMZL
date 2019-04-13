using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIManager:MonoSingleton<UIManager>
    {

        Dictionary<Type,UIWindow> cache;

        protected override void Initialize ()
        {
            cache = new Dictionary<Type,UIWindow>();           
        }

        //where T:UIWindow 含义:约束T类型必须是UIWindow的子类
        /// <summary>
        /// 获取UI窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetWindow<T> () where T : UIWindow
        {
            Type type = typeof(T);
            //如果没有type,则返回空
            if ( !cache.ContainsKey(type) ) return null;
            return cache[type] as T;
        }

        /// <summary>
        /// 注册UI
        /// </summary>
        /// <param name="ui"></param>
        public void RegisterUI ( UIWindow ui )
        {
            cache[ui.GetType()] = ui;
        }

        /// <summary>
        /// 清空UI字典
        /// </summary>
        public void ClearUICache ()
        {
            cache.Clear();
        }

    }
}
