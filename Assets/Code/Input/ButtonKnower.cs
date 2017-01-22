using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code.Input
{
    class ButtonKnower : MonoBehaviour, IResolveable
    {
        // http://answers.unity3d.com/questions/784617/how-do-i-block-touch-events-from-propagating-throu.html
        public bool WasJustADamnedButton()
        {
            var ct = EventSystem.current;

            if (!ct.IsPointerOverGameObject()) return false;
            if (!ct.currentSelectedGameObject) return false;
            if (ct.currentSelectedGameObject.GetComponent<Button>() == null)
                return false;

            return true;
        }
    }
}
