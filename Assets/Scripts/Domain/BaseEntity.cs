using System;
using System.Collections.Generic;
using UnityEngine;

namespace Motto.Domain
{


    [Serializable]
    public abstract class BaseEntity
    {

        public long id;

        public DateTime createdDate;

        public DateTime lastModifiedDate;

        public String modifiedBy;

        public String name;

        public string thumbnailFileUrl;

        public MottoVector3 position;

        public MottoVector3 rotation;

        public MottoVector3 scale;

        public void ExtractFromGameObject(GameObject gameObject)
        {
            position = new MottoVector3(gameObject.transform.position);
            rotation = new MottoVector3(gameObject.transform.rotation.eulerAngles);
            scale = new MottoVector3(gameObject.transform.localScale);
        }

        public void ExtractFromTransform(Transform transform)
        {
            position = new MottoVector3(transform.position);
            rotation = new MottoVector3(transform.rotation.eulerAngles);
            scale = new MottoVector3(transform.localScale);
        }

    }


}
