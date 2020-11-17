using System;
using System.Collections.Generic;

namespace Motto.Domain
{
    [Serializable]
    public class CatalogCategory : BaseEntity
    {

        public List<Motto.Domain.CatalogProduct> products = new List<Motto.Domain.CatalogProduct>();
        public string imageUrl;
        public List<CatalogCategory> children;
        public string type;
        public int catelogIndex;
        public string parentId;
        public CatalogCategory()
        {

        }

        public CatalogCategory(long id)
        {
            this.id = (id);
        }
    }
}

