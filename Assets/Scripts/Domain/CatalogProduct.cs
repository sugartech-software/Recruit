using System;
using System.Collections.Generic;

namespace Motto.Domain
{

    [Serializable]
    public class CatalogProduct : BaseEntity
    {


        public User user = new User();

        public List<String> imageUrlList;
        public String fileUrl;
        public String gltfFileUrl = "";
        public String usdzFileUrl = "";
        public String fileName;
        public Double xWidth;
        public Double yHeight;
        public Double zDepth;
        public String productDetail;
        public String platform;
        public int colliderType;
        public int placeType;
        public string type;
        // DUvar kagidi ve seramik icin
        public String productTextureFile;

        public CatalogCategory category;

        public Status status;

        public List<Comment> comments = new List<Comment>();

        public List<Motto.Domain.ProductPart> productParts = new List<Motto.Domain.ProductPart>();

        public CatalogProduct()
        {

        }

        public CatalogProduct(long id)
        {
            this.id = id;
        }
    }

}