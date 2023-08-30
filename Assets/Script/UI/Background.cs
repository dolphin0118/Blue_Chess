using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Background : MonoBehaviour {
    Image backgroundImage;
    
    void Awake(){   
        backgroundImage = GetComponent<Image>();
        Init();
    }

    void Init() {
        backgroundImage.GetComponent<Image>().rectTransform.SetAsFirstSibling();
    }

}
