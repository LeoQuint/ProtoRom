using UnityEngine;
using System.Collections;

public static class Extensions {

    public static IEnumerator SetActive(this GameObject go, bool status, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(status);
    }

}
