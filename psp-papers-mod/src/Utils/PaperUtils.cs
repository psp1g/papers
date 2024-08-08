using app;
using System.Diagnostics.CodeAnalysis;

namespace psp_papers_mod.Utils;

[SuppressMessage("ReSharper", "InconsistentNaming")] // Haxe extensions are lowercase
public static class PaperUtils {

    public static int nextInt(this Rand rand, int bound) {
        return System.Math.Abs(rand.nextInt() % bound);
    }
    
    public static int nextInt(this Rand rand, int min, int max) {
        return System.Math.Abs(rand.nextInt() % (max - min)) + min;
    }
    
}