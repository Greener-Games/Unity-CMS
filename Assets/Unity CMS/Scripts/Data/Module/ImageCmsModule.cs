using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    public class ImageCmsModule : CmsModule
    {
        public Color color;

        public override void Apply(GameObject content)
        {
            Image img = content.GetComponent<Image>();
            if (img != null)
            {
                img.color = color;
            }
        }

        public override void Populate(GameObject content)
        {
            Image img = content.GetComponent<Image>();
            if (img != null)
            {
                color = img.color;
            }
        }
}
}