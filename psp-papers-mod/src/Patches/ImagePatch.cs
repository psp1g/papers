using app.vis;
using haxe.io;
using System.IO;
using HarmonyLib;
using Cpp2IL.Core.Extensions;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace psp_papers_mod.Patches;


/// <summary>
///     This patch is a workaround to reverse-engineering the game's "art.dat" file, which is a heavily compressed (or encrypted?)
///     binary file containing all the art assets for the game.
/// </summary>

[HarmonyPatch(typeof(Image))]
public class ImagePatch {

    private static int ct;

    [HarmonyPrefix]
    [HarmonyPatch("fromPng", typeof(Bytes))]
    private static void FromPngPrefix(ref Bytes bytes) {
        Directory.CreateDirectory("img/");

        int cCt = ++ct;

        using FileStream fsw = new($"img/{cCt}.png", FileMode.Create);
        fsw.Write(bytes.b, 0, bytes.b.Length);

        string patchPath = $"img_patch/{cCt}.png";
        // Replace the image if there is one available
        if (!File.Exists(patchPath)) return;

        using FileStream fsr = new(patchPath, FileMode.Open);

        byte[] rBytes = fsr.ReadBytes();
        Il2CppStructArray<byte> il2cppBytes = new(rBytes);

        bytes = new Bytes(il2cppBytes.Length, il2cppBytes);
    }

}