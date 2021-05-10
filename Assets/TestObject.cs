using GG.UnityCMS;
using UnityEngine.UI;

[Data(typeof(TestData))]
public class TestObject : CmsGameObject<TestData>
{
    protected override void Apply(TestData content)
    {
        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.color = content.color;
        }
    }


}