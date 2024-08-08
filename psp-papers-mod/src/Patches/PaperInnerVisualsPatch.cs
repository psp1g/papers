using app.vis;
using data;
using HarmonyLib;
using haxe.xml._Access;
using play.day;
using psp_papers_mod.Utils;
using System;
using Image = app.vis.Image;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Mark))]
public class MarkPatch {
    [HarmonyPostfix]
    [HarmonyPatch("__hx_ctor_data_Mark")]
    public static void PrepareVerticalVisuals(object[] __args) {
        Mark mark = __args[0] as Mark;
        if (mark!.type != MarkType.TEXT) return;

        Xml markNode = __args[1] as Xml;
        if (!HasAttribAccess_Impl_.resolve(markNode, "vertical")) return;

        string value = AttribAccess_Impl_.resolve(markNode, "vertical");
        string prepend = value switch {
            "up" => "__VERT_U__",
            "down" => "__VERT_D__",
            _ => ""
        };

        mark.text = prepend + mark.text;
    }
}

[HarmonyPatch(typeof(PaperInnerVisuals))]
public class PaperInnerVisualsPatch {
    [HarmonyPostfix]
    [HarmonyPatch("__hx_ctor_play_day_PaperInnerVisuals")]
    private static void CtorPostfix(object[] __args) {
        Array visuals = (__args[0] as PaperInnerVisuals)!.visuals;
        BoothEnv boothEnv = (__args[1] as BoothEnv)!;
        PaperDef paperDef = (__args[2] as PaperDef)!;
        int length = visuals.length;
        for (int i = 0; i < length; i++) {
            Text text = visuals.__get(i).TryCast<Text>();
            if (text == null) continue;
            if (!text.text.StartsWith("__VERT_")) continue;
            bool bottomToTop = text.text.StartsWith("__VERT_U__");

            PointData originalPos = new(text.get_x(), text.get_y());

            string newText = text.text[10..];
            if (newText.StartsWith("$")) {
                newText = boothEnv.getLocalizedText(paperDef.id, newText[1..]);
            }
            newText = newText.Replace("\\n", "\n");
            
            text.set_text(newText);
            text.set_y(0);
            text.set_x(0);
            text.visible = true;
            text.generation += 1;
            text.buildTiles();

            Image spriteImg = new(text.builtWidth + 1, text.builtHeight + 1, Alloc.NEW);
            text.renderToImage(spriteImg, new PointData(0, 0), PasteMode.OVERLAY);
            spriteImg = bottomToTop ? spriteImg.rotatedLeft90() : spriteImg.rotatedRight90();

            Sprite sprite = new(new PartData(spriteImg, new Rect(
                0.0.ToIl2CppBoxed(),
                0.0.ToIl2CppBoxed(),
                ((double)spriteImg.width).ToIl2CppBoxed(),
                ((double)spriteImg.height).ToIl2CppBoxed()
            ))) {
                visible = true,
                scaleX = 1,
                scaleY = 1,
                pos = originalPos,
                hostPos = text.hostPos
            };

            if (text.align == Align.CENTER) {
                sprite.set_y(sprite.get_y() + text.builtHeight / 2.0);
            }

            visuals.__set(i, sprite);
        }
    }
}