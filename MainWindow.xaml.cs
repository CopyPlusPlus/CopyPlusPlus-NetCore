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

            // Is the content copied of text type?
            if (e.ContentType == SharpClipboard.ContentTypes.Text)
            {
                // Get the cut/copied text.
                string text = e.Content.ToString();

                for (int counter = 0; counter < text.Length - 1; counter++)
                {
                    //Console.WriteLine(text[counter]);
                    //if (text[counter + 1].ToString() == "\r" && text[counter].ToString() != "。")
                    //{
                    //    text = text.Remove(counter + 1, 2);
                    //}
                    //if (text[counter + 1].ToString() == "\r" && text[counter].ToString() != ".")
                    //{
                    //    text = text.Remove(counter + 1, 2);
                    //}

                    if (text[counter + 1].ToString() == "\r")
                    {
                        if (text[counter].ToString() == ".")
                        {
                            continue;
                        }
                        if (text[counter].ToString() == "。")
                        {
                            continue;
                        }
                        text = text.Remove(counter + 1, 2);
                    }               
                }

                //string text_after = text.Replace("\n","").Replace("\r","");

                Clipboard.SetText(text);
            }

            // Is the content copied of image type?
            else if (e.ContentType == SharpClipboard.ContentTypes.Image)
            {
                //do nothing

                // Get the cut/copied image.
                //Image img = (Image)e.Content;
            }

            // Is the content copied of file type?
            else if (e.ContentType == SharpClipboard.ContentTypes.Files)
            {
                //do nothing

                // Get the cut/copied file/files.
                //Debug.WriteLine(clipboard.ClipboardFiles.ToArray());

                // ...or use 'ClipboardFile' to get a single copied file.
                //Debug.WriteLine(clipboard.ClipboardFile);

            }

            // If the cut/copied content is complex, use 'Other'.
            else if (e.ContentType == SharpClipboard.ContentTypes.Other)
            {
                //do nothing

                // Do something with 'clipboard.ClipboardObject' or 'e.Content' here...
            }



        }

        private void Todolist_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("欢迎赞助我，加快开发进度！");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://github.com/CopyPlusPlus/CopyPlusPlus");
        }
    }
}
