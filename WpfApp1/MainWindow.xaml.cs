using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WpfApp1 {
    public partial class MainWindow : Window {

        static string[] lines;
        static int currIndex;
        public MainWindow() {
            InitializeComponent();
            Loaded += YourWindow_Loaded;
            Unloaded += YourWindow_Unloaded;

            string pathIn = @"D:\Programmmieren\Projects\ShrekScript as DC aka\Many brave knigts had attempted to .txt";
            string pathOut = @"D:\Programmmieren\Projects\ShrekScript as DC aka\out.txt";
            string lastSuccsesfulLine = "Next! -Get up! Come on! -Twenty ";
            string text = File.ReadAllText(pathIn);

            text = text.Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\r", " ");
            text = Regex.Replace(text, @"\s+", " ");
            text = InsertNewLines(text, 32);
            lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            TextBox textBlock = new();
            MainCanvas.Children.Add(textBlock);
            textBlock.Text = lines.ToList().IndexOf(lastSuccsesfulLine).ToString();
            textBlock.Text += Environment.NewLine + lines.Length;
            textBlock.Text += Environment.NewLine + ((double)lines.ToList().IndexOf(lastSuccsesfulLine) / (double)lines.Length).ToString();
            textBlock.Text += Environment.NewLine + ((double)lines.ToList().IndexOf(lastSuccsesfulLine) / (double)lines.Length * 100).ToString();
        }

        private void YourWindow_Loaded(object sender, RoutedEventArgs e) {
            hookId = SetHook(KeyboardHookProc);
        }

        private void YourWindow_Unloaded(object sender, RoutedEventArgs e) {
            UnhookWindowsHookEx(hookId);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc) {
            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule) {
                keyboardProc = proc;
                return SetWindowsHookEx(WH_KEYBOARD_LL, keyboardProc, GetModuleHandle(currentModule.ModuleName), 0);
            }
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_F11 = 0x7A;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc keyboardProc;

        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN) {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == VK_F11) {
                    // React to F11 key press
                    // Add your code here to handle the event
                    Clipboard.SetText(lines[currIndex]);
                    currIndex++;
                }
            }

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }


        private static string InsertNewLines(string text, int charactersPerLine) {
            StringBuilder result = new StringBuilder();
            int lineLength = 0;

            string[] words = text.Split(' ');

            foreach (string word in words) {
                if (lineLength + word.Length > charactersPerLine) {
                    result.AppendLine();
                    lineLength = 0;
                }

                result.Append(word + " ");
                lineLength += word.Length + 1;
            }

            return result.ToString();
        }
    }
}
