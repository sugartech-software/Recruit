using System;
using System.Collections.Generic;

public class StringUtils
{

    public static bool Contains(string container, List<string> containList)
    {
        return Contains(container, containList.ToArray());
    }

    public static bool Contains(string container, params string[] containList)
    {
        for(int i = 0; i < containList.Length; i++) {
            if (container.Contains(containList[i]))
                return true;
        }
        return false;
    }
}

