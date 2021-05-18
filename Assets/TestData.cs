using System.Collections;
using System.Collections.Generic;
using GG.UnityCMS;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TestData : CmsModuleData
{
    public Color color = Color.white;
    
    public override void Populate(CmsGameObject content)
    {
        Image img = content.GetComponent<Image>();
        if (img != null)
        {
            color = img.color;
        }
    }
}