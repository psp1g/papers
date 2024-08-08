using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System;

namespace psp_papers_mod.Utils;

public static class Il2CppUtils {
    public static Array NewHaxeArray(uint size) {
        return new Array(new Il2CppReferenceArray<Il2CppSystem.Object>(size));
    }

    public static Array HaxeArrayOf<T>(params T[] elements) where T : Il2CppSystem.Object {
        Array array = new Array(new Il2CppReferenceArray<Il2CppSystem.Object>(elements.Length));
        for (int i = 0; i < elements.Length; i++) {
            array.__set(i, elements[i]);
        }

        return array;
    }

    public static Il2CppSystem.Object ToIl2CppBoxed(this bool boolean) {
        return new Il2CppSystem.Boolean { m_value = boolean }.BoxIl2CppObject();
    }
    
    public static Il2CppSystem.Object ToIl2CppBoxed(this short num) {
        return new Il2CppSystem.Int16 { m_value = num }.BoxIl2CppObject();
    }

    public static Il2CppSystem.Object ToIl2CppBoxed(this int num) {
        return new Il2CppSystem.Int32 { m_value = num }.BoxIl2CppObject();
    }

    public static Il2CppSystem.Object ToIl2CppBoxed(this long num) {
        return new Il2CppSystem.Int64 { m_value = num }.BoxIl2CppObject();
    }

    public static Il2CppSystem.Object ToIl2CppBoxed(this float num) {
        return new Il2CppSystem.Single { m_value = num }.BoxIl2CppObject();
    }

    public static Il2CppSystem.Object ToIl2CppBoxed(this double num) {
        return new Il2CppSystem.Double{ m_value = num }.BoxIl2CppObject();
    }
    
    public static bool IsType<T>(this Il2CppSystem.Object obj) {
        IntPtr nativeClassPtr = Il2CppClassPointerStore<T>.NativeClassPtr;
        IntPtr objClassPtr = IL2CPP.il2cpp_object_get_class(obj.Pointer);
        return IL2CPP.il2cpp_class_is_assignable_from(nativeClassPtr, objClassPtr);
    }
    
}