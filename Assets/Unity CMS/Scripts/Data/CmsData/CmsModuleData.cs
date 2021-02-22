using System;
using System.Linq;
using GG.UnityCMS;

[Serializable]
public abstract class CmsModuleData
{
    string name = "";

    public string Name
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Concat(GetType().Name.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
            }

            return name;
        }
    }

    public bool foldout;

    public abstract void Populate(CmsGameObject component);

    //TODO: need to think of a better way to link the editor without having it embedded in the class structure
    public abstract void DrawEditor();

    public override string ToString()
    {
        return Name;
    }
}