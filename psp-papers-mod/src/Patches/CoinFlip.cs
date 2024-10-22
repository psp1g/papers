using app.vis;
using HarmonyLib;
using play.day.booth;
using UnityEngine;
using psp_papers_mod;
using psp_papers_mod.Patches;

// using different namespace to keep all the patches relevant to this feature in the same file, idk if this is a good approach
// idk if having two patches for the same class in different places could cause issues, it didn't so far

namespace Coin {

    public class CoinFlip {

        static Paper coin;

        public static bool isAnimating = false;
        static float animStart;
        static float animEnd;
        static float framecount = 0;
        static int duration = 1;
        static int animSpeedOffset = 1;  // higher = slower
        static PointData movementDir = new PointData(0, 0);

        enum Pages {
            Heads,
            HeadsAngleL,
            Side,
            TailsAngleR,
            Tails,
            TailsAngleL,
            Side2,
            HeadsAngle,
        }

        public static void Reset() {
            coin = null;
            isAnimating = false;
        }
        
        static bool SetCoinInstance() {
            if (coin == null) {
                coin = BorderPatch.Border.booth.autoFindPaper("Coin");
                if (coin == null) return false;
                movementDir = coin.deskItem.pos;
            }
            return true;

        }

        public static void onClick() {

            if (!SetCoinInstance()) return;

            if (coin.idWithIndex == "Coin") {
                if (isAnimating) {
                    isAnimating = false;
                    BorderPatch.Border.day.endless.speaker.play("metal-dragstart");

                    coin.deskItem.touchDrag.decay = 0.75;

                    // determine result based on current frame 
                    if (coin.get_page() <= (int)Pages.HeadsAngleL)
                        coin.set_page((int)Pages.Heads);
                    if (coin.get_page() >= (int)Pages.Side)
                        coin.set_page((int)Pages.Tails);
                    if (coin.get_page() >= (int)Pages.Side2)
                        coin.set_page((int)Pages.Heads);
                }
            }
        }

        public static void NextFrame() {

            if (!isAnimating)
                return;

            int secondsElapsed = 1 + (int)(Time.realtimeSinceStartup - animStart);
            framecount++;

            // animation speed based on time elapsed and throwing speed
            if (framecount % (secondsElapsed + animSpeedOffset) == 0) {
                var pages = coin.def.pages;
                int index = (coin.get_page() + 1) % pages.length;
                coin.setPage(index);

                BorderPatch.Border.day.endless.speaker.play("metal-dragstop");
            }

            // change movementdir every 5 frames
            if (framecount % 5 == 0) {
                PointData pos = coin.deskItem.pos;
                pos.x += PapersPSP.Random.Next(-2, 3);
                pos.y += PapersPSP.Random.Next(-1, 2);
                movementDir = pos;
            }

            // apply movementdir every frame
            coin.deskItem.pos = movementDir;



            if (secondsElapsed > duration) {
                Stop();
            }
        }

        static void Animate() {
            isAnimating = true;
            coin.deskItem.touchDrag.decay = 0.85;
            framecount = 0;
            animStart = Time.realtimeSinceStartup;
            BorderPatch.Border.day.endless.speaker.play("metal-dragstart");

        }

        public static void Start() {
            if (!SetCoinInstance()) return;

            PointData vel = coin.deskItem.touchDrag.vel;

            double normalizedVel = Math.sqrt(Math.pow(vel.x, 2) + Math.pow(vel.y, 2));

            int max = 3500;  // estimate based on flicking a bunch of times
            normalizedVel = Math.abs(normalizedVel / max); // between 0 and 1 (can go above 1)

            if (normalizedVel > 0.2f) {

                // magic values that feel good, if changing should change both
                double speed = (1.0f / normalizedVel);
                animSpeedOffset = (int)speed;
                duration = (int)(10.0f / speed);
                PapersPSP.Log.LogWarning(duration);

                Animate();
            }
        }

        static void Stop() {

            // don't stop on "angle" frames
            int index = coin.get_page();
            data.Page page = coin.def.pages[index].TryCast<data.Page>();
            if (page.id.Contains("Angle"))
                return;

            BorderPatch.Border.day.endless.speaker.play("metal-dragstart");


            // make landing on side more rare 
            if (page.id.Contains("side")) {
                int random = PapersPSP.Random.Next(3);
                if (random == 0) {
                    if (coin.get_page() == (int)Pages.Side)
                        coin.set_page((int)Pages.Tails);
                    if (coin.get_page() == (int)Pages.Side2)
                        coin.set_page((int)Pages.Heads);
                }
            }


            isAnimating = false;
            coin.deskItem.touchDrag.decay = 0.75;

            PapersPSP.Log.LogInfo("Coin result: " + page.id);

            //can also land on side if the animation stops there
        }
    }

    [HarmonyPatch(typeof(Paper))]
    internal class PaperPatch {

        [HarmonyPostfix]
        [HarmonyPatch("onClick", typeof(Visual))]
        static void clickCoin(Visual hitVisual) {
            CoinFlip.onClick();
        }
    }

    [HarmonyPatch(typeof(TouchDrag))]
    internal class TouchDragPatch {
        [HarmonyPostfix]
        [HarmonyPatch("stop")]
        static void moveCoin() {
            CoinFlip.Start();
        }

        [HarmonyPostfix]
        [HarmonyPatch("start")]
        static void stopCoin() {
            if (CoinFlip.isAnimating) {
                CoinFlip.onClick();
            }
        }
    }  
    
  

    [HarmonyPatch(typeof(Booth))]
    internal class BoothPatch {
        [HarmonyPostfix]
        [HarmonyPatch("draw")]
        private static void nextCoinFrame() {
            CoinFlip.NextFrame();
        }
    }
}

