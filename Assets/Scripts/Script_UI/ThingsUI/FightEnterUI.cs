using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnterUI : MonoBehaviour {
    public GameObject Button_lianShou;
    public GameObject Button_biSai;
    public GameObject Button_boDou;
    public GameObject Button_zhongShang;
    public GameObject Button_siDou;

    public GameObject Button_Loot;
    public GameObject Button_Pretend;
    public GameObject Button_GiveUp;

    GameObject loot_Choose;
    GameObject pretend_Choose;
    bool isLoot;
    bool isPretent;

    void Start () {

    }
	

	void Update () {
		
	}

    /// <summary>
    /// ѡ��ս����ͼ
    /// </summary>
    /// <param name="lianshou">�Ƿ��������֣�������˺�</param>
    /// <param name="bisai">�Ƿ�������������Ѫ��������</param>
    /// <param name="bodou">�Ƿ��������������Ϊ����</param>
    /// <param name="zhongshang">�Ƿ��������ˣ����Ϊ����</param>
    /// <param name="sidou">�Ƿ��������������Ϊ����</param>
    /// <param name="loot">�Ƿ��������ӣ����öԷ����ߣ����ӷ��Ｘ��</param>
    /// <param name="pretend">�Ƿ�����αװ������ٷ��Ｘ��</param>
    /// <param name="giveUp">�Ƿ������뿪</param>
    public void FightEnter(bool lianshou,bool bisai,bool bodou,bool zhongshang,bool sidou,bool loot, bool pretend, bool giveUp)
    {
        CommonFunc.GetInstance.SetButtonState(Button_lianShou, lianshou);
        CommonFunc.GetInstance.SetButtonState(Button_biSai, bisai);
        CommonFunc.GetInstance.SetButtonState(Button_boDou, bodou);
        CommonFunc.GetInstance.SetButtonState(Button_zhongShang, zhongshang);
        CommonFunc.GetInstance.SetButtonState(Button_siDou, sidou);
        CommonFunc.GetInstance.SetButtonState(Button_Loot, loot);
        CommonFunc.GetInstance.SetButtonState(Button_Pretend, pretend);
        CommonFunc.GetInstance.SetButtonState(Button_GiveUp, giveUp);

        UIEventListener.Get(Button_lianShou).onClick = FightStart;
        UIEventListener.Get(Button_biSai).onClick = FightStart;
        UIEventListener.Get(Button_boDou).onClick = FightStart;
        UIEventListener.Get(Button_zhongShang).onClick = FightStart;
        UIEventListener.Get(Button_siDou).onClick = FightStart;

        UIEventListener.Get(Button_Loot).onClick = Loot;
        UIEventListener.Get(Button_Pretend).onClick = Pretend;
        UIEventListener.Get(Button_GiveUp).onClick = FightOver;

        loot_Choose = Button_Loot.transform.Find("Sprite_Choose").gameObject;
        loot_Choose.SetActive(false);
        pretend_Choose = Button_Pretend.transform.Find("Sprite_Choose").gameObject;
        pretend_Choose.SetActive(false);
    }

    void ButtonTips(GameObject btn,bool isHover)
    {
        Transform lableTips = CommonFunc.GetInstance.UI_Instantiate(Data_Static.UIpath_LableTips, btn.transform, Vector3.zero, Vector3.one);
        if (isHover)
        {
            lableTips.GetComponent<LableTipsUI>().SetAll(false, "System_1", btn.name);
        }
        else
        {
            Destroy(lableTips.gameObject);
        }
    }

    void Loot(GameObject btn)
    {
        if (loot_Choose.activeSelf)
        {
            loot_Choose.SetActive(false);
        }
        else
        {
            loot_Choose.SetActive(true);
        }
    }
    void Pretend(GameObject btn)
    {
        if (pretend_Choose.activeSelf)
        {
            pretend_Choose.SetActive(false);
        }
        else
        {
            pretend_Choose.SetActive(true);
        }
    }

    void FightStart(GameObject btn)
    {
        Transform fighting = CommonFunc.GetInstance.UI_Instantiate(Data_Static.UIpath_Fighting, FindObjectOfType<UIRoot>().transform, Vector3.zero, Vector3.one);
        fighting.GetComponent<FightingUI>().FightStart(btn.name,isLoot,isPretent);

        Destroy(this.gameObject);
    }

    void FightOver(GameObject btn)
    {
        FindObjectOfType<Scene_City>().collision_Wall.GetComponent<PolygonCollider2D>().isTrigger = false;
        Destroy(this.gameObject);
        Destroy(FindObjectOfType<Scene_Fight>().gameObject);
    }
}
