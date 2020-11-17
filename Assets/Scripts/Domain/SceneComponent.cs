using System;

namespace Motto.Domain
{

    [Serializable]
    public class SceneComponent : BaseEntity
    {
        public string componentType;
        public string fileUrl;
        public string componentValue;
        public MottoVector3 color;
    }
}

