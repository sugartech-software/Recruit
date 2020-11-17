using System;
using System.Collections.Generic;


namespace Motto.Domain
{
    [Serializable]
    public class User: BaseEntity
    {


        public String firstName;

        public String lastName;

        public String designerName;

        public String imageUrl;

        public String email;

        public String password;

        public String rePassword;

        public bool isDesigner;

        public User representedUser;


        public User()
        {

        }

        public User(long id)
        {
            this.id = id;
        }


        public ISet<CatalogProduct> catalogProducts = new HashSet<CatalogProduct>();


        public void addProduct(CatalogProduct product)
        {
            addProduct(product, true);
        }

        public void addProduct(CatalogProduct product, bool set)
        {
            catalogProducts.Clear();
            if (product != null)
            {
                catalogProducts.Add(product);
                if (set)
                {
                    product.user = this;
                }
            }
        }

        public void removePart(CatalogProduct catalogProduct)
        {
            catalogProducts.Remove(catalogProduct);
            catalogProduct.user = null; ;
        }



        private List<Role> roles = new List<Role>();

       

    }
}