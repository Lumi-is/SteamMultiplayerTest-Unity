using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class PanelSwapper : MonoBehaviour
    {
        public List<Panel> panels = new();

        public void SwapPanelTo(string panelName)
        {
            foreach(Panel panel in panels)
            {
                if(panel.panelName == panelName)
                {
                    panel.gameObject.SetActive(true);
                } else
                {
                    panel.gameObject.SetActive(false);
                }
            }
        }
    }
}

