using Common;
using UnityEngine;

namespace DMZL_UI
{
    public class UI_SystemWindow :UIWindow
    {
        UI_Opinion[] opinions;
        public override void Initialize ()
        {
            opinions = GetComponentsInChildren<UI_Opinion>();
           
        }
        private void Start ()
        {
            opinions[0].FitVO(new VO_Opinion("WordTable_Test","AttackGate","AttackGateExplain",() =>Alert.Log("AttackGate")));
            opinions[1].FitVO(new VO_Opinion("WordTable_Test","LeaveGate","LeaveGateExplain",() => Alert.Log("LeaveGate")));
        }
    }
}
