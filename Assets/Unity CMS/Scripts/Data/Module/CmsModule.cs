using UnityEngine;

namespace GG.UnityCMS
{
    public abstract class CmsModule
    {
        public bool foldout;

        public void Apply(CmsGameObject component)
        {
            Apply(component.gameObject);
        }
        public abstract void Apply(GameObject component);

        public abstract void Populate(GameObject component);
    }

}