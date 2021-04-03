using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Windows;
using System.Windows.Input;
using WK.Libraries.SharpClipboardNS;

namespace CopyPlusPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Is the translate API being changed or not
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static bool changeStatus = false;
#pragma warning restore CA2211 // Non-constant fields should not be visible

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
                    if (switch1.IsChecked == true)
                    {
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

                    if (switch2.IsChecked == true)
                    {
                        if (text[counter].ToString() == " ")
                        {
                            text = text.Remove(counter, 1);
                        }
                    }
                }

                if (switch3.IsChecked == true)
                {
                    string appId = Properties.Settings.Default.AppID;
                    string secretKey = Properties.Settings.Default.SecretKey;

                    if (changeStatus == false)
                    {
                        if (appId == "none" || secretKey == "none")
                        {
                            MessageBox.Show("请先设置翻译接口");

                            KeyInput keyinput = new KeyInput();
                            keyinput.Show();
                            changeStatus = true;
                        }
                        else
                        {
                            text = BaiduTrans(appId, secretKey, text);
                        }

                    }
                }

                try
                {
                    Clipboard.SetText(text);
                }
                finally
                {

                }

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

        private string BaiduTrans(string appId, string secretKey, string q = "apple")
        {
            //q为原文

            // 源语言
            string from = "en";
            // 目标语言
            string to = "zh";

            // 改成您的APP ID
            //appId = NoAPI.baidu_id;
            // 改成您的密钥
            //secretKey = NoAPI.baidu_secretKey;

            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
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

            return result.trans_result[0].dst;
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

        //打开翻译按钮
        private void ShowInputWindowButton(object sender, RoutedEventArgs e)
        {
            string appId = Properties.Settings.Default.AppID;
            string secretKey = Properties.Settings.Default.SecretKey;
            if (appId == "none" || secretKey == "none")
            {
                MessageBox.Show("请先设置翻译接口");
                KeyInput keyinput = new KeyInput();
                keyinput.Show();
                changeStatus = true;
            }
        }

        //点击"翻译"文字
        private void ShowInputWindowText(object sender, MouseButtonEventArgs e)
        {
            KeyInput keyinput = new KeyInput();
            keyinput.Show();
            changeStatus = true;
        }
    }
}