using System;

namespace Motto.Domain
{
    [Serializable]
    public class CartelaFabric : BaseEntity
    {



        public string fileUrl;

        public string pbrFileUrl;

        public string cartelaId;

        public string files;

        public float gamma = 1.0f;


        public CartelaFabric()
        {

        }

        public CartelaFabric(long id)
        {
            this.id = id;
        }
    }
}

