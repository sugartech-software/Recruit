using UnityEngine;

public class VersionManager{
   
    private static VersionManager instance;

    public string VersionNo{
        get{
            return "1.0.0";
        }
    } 
    private VersionManager(){
        PlayerPrefs.SetString("VersionNo", VersionNo);
    }

    public static VersionManager Instance{
        get{
            if(instance == null){
                instance = new VersionManager();
             }
             return instance;
        }
        
    }



}
