using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Motto.Directory
{
    public class FileManager
    {
        private FileManager()
        {
        }

        private bool saveLock = false;

        private static Motto.Directory.FileManager instance;

        public static Motto.Directory.FileManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new Motto.Directory.FileManager();
                return instance;
            }
        }

        public IEnumerator SaveDirectory(string filePath, byte[] data)
        {

            MonoBehaviour.print("saving file length is " + data.Length);
            while (saveLock)
            {
                MonoBehaviour.print("saving is waitin for " + filePath);
                yield return null;
            }

            if (File.Exists(filePath))
            {
                yield break;
            }

            string filePathTmp = filePath + ".tmp";
            if (File.Exists(filePathTmp))
            {
                File.Delete(filePathTmp);
            }

            FileStream file = null;
            try
            {
                MonoBehaviour.print("saving is beginning for " + filePathTmp);
                saveLock = true;
                file = File.Create(filePathTmp);

                int offset = 0;
                int buffer = 1024 * 1024;

                while (offset + buffer <= data.Length)
                {
                    file.Write(data, offset, buffer);
                    offset += buffer;
                    //yield return null;
                }

                if (data.Length - offset > 0)
                {
                    file.Write(data, offset, data.Length - offset);
                    // yield return null;
                }


                file.Flush();
                file.Close();
            }
            catch
            {
                if (file != null)
                {
                    file.Close();
                }
            }



            //file.Write(data, 0, data.Length);
            MonoBehaviour.print("moving original name " + filePath);
            File.Move(filePathTmp, filePath);

            saveLock = false;
            yield return null;
            MonoBehaviour.print("saving is finished for " + filePath);
        }
    }
}



