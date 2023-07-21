using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltip : MonoBehaviour {
    GameObject tooltip;
    TextMeshProUGUI txt;
    RectTransform txt_rect;
    GameObject Tool_object;

    bool mouse_On;
    float tooltip_Count;
    float tooltip_Time = 1.0f;
    [TextArea]
    public string tooltipText;
    public string object_tag;

    private void TooltipBox_init() {
        tooltip = this.transform.parent.transform.Find("TooltipBox").gameObject;
        tooltip_Count = 0f;
        tooltip.SetActive(false);

        txt = tooltip.transform.Find("Tooltip").GetComponent<TextMeshProUGUI>();
        txt_rect = (RectTransform)tooltip.GetComponent<Image>().transform;
    }

    private void TooltipBox_Use() {
        if (mouse_On) {
            tooltip_Count += Time.deltaTime;
            if (tooltip_Count > tooltip_Time) {
                tooltip.SetActive(true);
                txt.text = tooltipText;
                Vector2 pos = (Vector2)this.transform.position + new Vector2(30, -50);
                tooltip.transform.position = pos;
                txt_rect.sizeDelta = new Vector2(txt.preferredWidth + 20, txt.preferredHeight + 20);
            }
        }

    }
}
