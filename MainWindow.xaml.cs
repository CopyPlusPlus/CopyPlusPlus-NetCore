using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using WK.Libraries.SharpClipboardNS;

namespace CopyPlusPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var clipboard = new SharpClipboard();
            // Attach your code to the ClipboardChanged event to listen to cuts/copies.
            clipboard.ClipboardChanged += ClipboardChanged;
        }

        private void ClipboardChanged(Object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            string text = e.Content.ToString();
            //string text_after = "";

            for (int counter = 0; counter < text.Length-1; counter++)
            {
                Console.WriteLine(text[counter]);
                if(text[counter+1].ToString() == "\r" && text[counter].ToString() != "。")
                {
                    text = text.Remove(counter+1, 2);
                }
            }
            
            //string text_after = text.Replace("\n","").Replace("\r","");

            Clipboard.SetText(text);

            //Debug.WriteLine(text);
            //Debug.WriteLine(text_after);
        }

        private void Todolist_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("你是想帮我完成这个功能吗?");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe","https://github.com/CopyPlusPlus/CopyPlusPlus");
        }
    }
}
