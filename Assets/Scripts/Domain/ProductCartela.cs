using System;
using System.Collections.Generic;


namespace Motto.Domain {
    [Serializable]
    public class ProductCartela : BaseEntity
    {
        public List<CartelaFabric> fabricList;

        public string imageUrl;

        public string file;

        public ProductCartela()
        {

        }

        public ProductCartela(long id)
        {
            this.id = id;
        }
    }
}



