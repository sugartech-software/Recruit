using System;
using System.Collections;

public interface  AsyncLoad<T>
{

      IEnumerator FinishLoad(T load);

      IEnumerator ErrorLoad(T load, string error);

      IEnumerator ProgressLoad(T load, string progress);

}
