using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace AutoScreenShot
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static UnityModManager.ModEntry mod;
        public static Harmony harmony;
        public static bool IsEnabled = false;
        public static Setting setting;
        /*
        public static void LoadDll()
        {
            string basePath = "Mods/AutoScreenShot/Forms.dll";
            FileStream fs = new FileStream(basePath, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            AppDomain.CurrentDomain.Load(buffer);
            Main.Logger.Log("Dll load : " + basePath);
        }*/

        public static bool Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate; 

            Main.harmony = new Harmony(modEntry.Info.Id);
            Main.mod = modEntry;
            setting = new Setting();
            setting = UnityModManager.ModSettings.Load<Setting>(modEntry);

            return true;
        }


        private static void OnUpdate(UnityModManager.ModEntry modentry, float deltaTime)
        {
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;

            if (value)
            {
                //켜질때
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;
            }
            else
            {
                //꺼질때
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            bool toggleOnDeath = GUILayout.Toggle(setting.onDeath, "죽었을 때 스크린샷 찍기");
            if (toggleOnDeath)
            {
                setting.onDeath = true;
            }
            if (!toggleOnDeath)
            {
                setting.onDeath = false;
            }

            bool toggleOnComplete = GUILayout.Toggle(setting.onComplete, "레벨을 클리어 했을 때 스크린샷 찍기");
            if (toggleOnComplete)
            {
                setting.onComplete = true;
            }
            if (!toggleOnComplete)
            {
                setting.onComplete = false;
            }

            bool toggleClipboard = GUILayout.Toggle(setting.saveClipboard, "캡처한 사진을 클립보드에 복사하기");
            if (toggleClipboard)
            {
                setting.saveClipboard = true;
            }
            if (!toggleClipboard)
            {
                setting.saveClipboard = false;
            }

            bool togglefile = GUILayout.Toggle(setting.saveFiles, "캡처한 사진을 파일로 저장하기");
            if (togglefile)
            {
                setting.saveFiles = true;
            }
            if (!togglefile)
            {
                setting.saveFiles = false;
            }

            GUILayout.Label(" ");
            if (GUILayout.Button("스크린샷 폴더 열기"))
            {
                System.Diagnostics.Process.Start("AutoScreenShots");
            }
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            setting.Save(modEntry);
        }
    }
}
