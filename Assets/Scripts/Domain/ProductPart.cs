using System;

namespace Motto.Domain {


    [Serializable]
    public class ProductPart: BaseEntity
    {

        public string xCoord;

        public string yCoord;

        public string zCoord;

        public float uvScale;

        public long fabricId;

        public CartelaFabric fabric;

        public long cartelaId;
        public ProductCartela cartela;

        public ProductPart()
        {

        }

        public ProductPart(long id)
        {
            this.id = id;
        }
    }
}
