using HarmonyLib;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace AutoScreenShot
{
    public static class Patch
    {
        public static bool isDeath = false;
        public static bool isClear = false;
        public static async void Delay()
        {
            await Task.Delay(1);
            Bitmap bmp = new Bitmap(Screen.width, Screen.height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);

            DirectoryInfo startdi = new DirectoryInfo(@"AutoScreenShots");
            if (!startdi.Exists)
            {
                startdi.Create();
            }

            if (Main.setting.saveFiles)
            {
                string file = @"AutoScreenShots\" + System.DateTime.Now.ToString("yyyy-MM-dd tt hh.mm.ss") + ".png";
                bmp.Save(file, ImageFormat.Png);
            }

            if (Main.setting.saveClipboard)
            {
                System.Windows.Forms.Clipboard.SetImage(bmp);
            }
            isDeath = false;
            isClear = false;
        }


        [HarmonyPatch(typeof(scrController), "PlayerControl_Update")]
        internal static class ScreenshotPatch
        {
            [HarmonyPatch(typeof(scrController), "Fail2Action")]
            private static void Postfix()
            {
                if (Main.setting.percent <= Main.Progress() && Main.setting.onDeath)
                {
                    isDeath = true;
                    Delay();
                }
            }
        }

        [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
        public static class ClearPatch
        {
            public static void Postfix(scrController __instance)
            {
                if (__instance.gameworld && Main.setting.onComplete)
                {
                    isClear = true;
                    Delay();
                }
            }
        }

        [HarmonyPatch(typeof(scrController), "Won_Update")]
        public static class WonPatch
        {
            public static bool Prefix()
            {
                if (isClear)
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(scrController), "ResetCustomLevel")]
        public static class ResetPatch
        {
            public static bool Prefix()
            {
                if (isDeath)
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(scrController), "Restart")]
        public static class ResetPatch2
        {
            public static bool Prefix()
            {
                if (isDeath)
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(scrController), "TogglePauseGame")]
        public static class PausePatch
        {
            public static bool Prefix()
            {
                if (isDeath || isClear)
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(scnEditor), "Update")]
        public static class EditorPatch
        {
            public static bool Prefix()
            {
                if (isDeath || isClear)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
