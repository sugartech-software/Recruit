using System;

    public class StreamFactoryManager
    {

        private StreamFactory instance;
        private StreamFactoryManager()
        {

        }

        public StreamFactory GetStreamFactory()
        {
            if(instance == null)
            {
                instance = new FileStreamFactory();
            }

            return instance;
        }
    }
