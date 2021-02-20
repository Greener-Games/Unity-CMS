using UnityEngine;
using UnityEngine.UI;

namespace GG.UnityCMS
{
    public class ImageCmsModule : CmsModule
    {
        public Color color;

        public void Apply(Image img)
        {
            if (img != null)
            {
                img.color = color;
            }
        }

        public ImageCmsModule Set(Image img)
        {
            if (img != null)
            {
                color = img.color;
            }
            return this;
        }
}
}