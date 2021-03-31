using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
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

        private void Github_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://github.com/CopyPlusPlus/CopyPlusPlus");
        }

        private void BaiduTrans(object sender, RoutedEventArgs e)
        {
            // 原文
            string q = "apple";
            // 源语言
            string from = "en";
            // 目标语言
            string to = "zh";
            // 改成您的APP ID
            string appId = API.baidu_id;
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = API.baidu_secretKey;
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            //read json(retString) as a object
            var result = JsonSerializer.Deserialize<Rootobject>(retString);
            
            Console.WriteLine(retString);
            Console.WriteLine(result.trans_result[0].dst);

        }
        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

    }
}
