using System.Collections.Generic;
using System.Linq;

namespace psp_papers_mod;

public static class Extensions {

    public static T Random<T>(this IEnumerable<T> collection) {
        T[] arr = collection.ToArray();
        int i = PapersPSP.Random.Next(arr.Length);
        return arr[i];
    }

}