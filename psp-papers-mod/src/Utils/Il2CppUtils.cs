using haxe.ds;
using haxe.ds._List;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using System.Collections.Generic;
using Object = Il2CppSystem.Object;

namespace psp_papers_mod.Utils;

public static class Il2CppUtils {
    public static Array NewHaxeArray(uint size) {
        return new Array(new Il2CppReferenceArray<Object>(size));
    }

    public static Array HaxeArrayOf<T>(params T[] elements) where T : Object {
        Array array = new Array(new Il2CppReferenceArray<Object>(elements.Length));
        for (int i = 0; i < elements.Length; i++) {
            array.__set(i, elements[i]);
        }

        return array;
    }

    public static Object ToIl2CppBoxed(this bool boolean) {
        return new Boolean { m_value = boolean }.BoxIl2CppObject();
    }
    
    public static Object ToIl2CppBoxed(this short num) {
        return new Int16 { m_value = num }.BoxIl2CppObject();
    }

    public static Object ToIl2CppBoxed(this int num) {
        return new Int32 { m_value = num }.BoxIl2CppObject();
    }

    public static Object ToIl2CppBoxed(this long num) {
        return new Int64 { m_value = num }.BoxIl2CppObject();
    }

    public static Object ToIl2CppBoxed(this float num) {
        return new Single { m_value = num }.BoxIl2CppObject();
    }

    public static Object ToIl2CppBoxed(this double num) {
        return new Double{ m_value = num }.BoxIl2CppObject();
    }

    public static Object ToIl2Cpp(this string str) {
        return new Object(IL2CPP.ManagedStringToIl2Cpp(str));
    }
    
    public static string ToManagedString(this Object obj) {
        return IL2CPP.Il2CppStringToManaged(obj.Pointer);
    }
    
    public static List<Object> ToIlList(this List list) {
        List<Object> netList = [];
        ListIterator iter = list.iterator();
        while (iter.hasNext()) {
            netList.Add(iter.next());
        }

        return netList;
    }
    
    public static bool IsType<T>(this Object obj) {
        System.IntPtr nativeClassPtr = Il2CppClassPointerStore<T>.NativeClassPtr;
        System.IntPtr objClassPtr = IL2CPP.il2cpp_object_get_class(obj.Pointer);
        return IL2CPP.il2cpp_class_is_assignable_from(nativeClassPtr, objClassPtr);
    }
    
}