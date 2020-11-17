using System;

namespace Motto.Domain
{

    [Serializable]
    public class Comment : BaseEntity
    {

        public String message;

        public CatalogProduct product;


        public Comment()
        {

        }

        public Comment(long id)
        {
            this.id = id;
        }

    }
}