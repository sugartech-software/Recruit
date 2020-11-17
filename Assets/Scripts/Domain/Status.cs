using System;

namespace Motto.Domain
{

    [Serializable]
    public class Status : BaseEntity
    {

        public Status()
        {

        }

        public Status(long id)
        {
            this.id = id;
        }

       
    }
}