using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable UI Text Button", menuName = "Scriptable/UI/Text Button")]
    public class SCRUITextButton : SCRUIBaseButton
    {
        [Tooltip("text of button")]
        public string Text;
    }
}