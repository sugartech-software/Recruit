

using System;

namespace Motto.Domain
{
    [Serializable]
    public class Render : BaseEntity
    {
        public Render()
        {

        }
        public string uuid;

        public string renderRequest;
        public string status;

        public string output;

        public string outputS3Path;


    }
}
