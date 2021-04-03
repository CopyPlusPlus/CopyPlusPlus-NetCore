using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CopyPlusPlus
{
    /// <summary>
    /// Interaction logic for KeyInput.xaml
    /// </summary>
    public partial class KeyInput : Window
    {
        // Holds a value determining if this is the first time the box has been clicked
        // So that the text value is not always wiped out.
        bool hasBeenClicked1 = false;
        bool hasBeenClicked2 = false;

        public KeyInput()
        {
            InitializeComponent();
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Name == "textBox1")
            {
                if (!hasBeenClicked1)
                {
                    box.Text = String.Empty;
                    hasBeenClicked1 = true;
                }
            }
            if (box.Name == "textBox2")
            {
                if (!hasBeenClicked2)
                {
                    box.Text = String.Empty;
                    hasBeenClicked2 = true;
                }
            }
        }

        private void WriteKey(object sender, EventArgs e)
        {
            if (textBox1.Text != "关闭窗口自动保存")
            {
                Properties.Settings.Default.AppID = textBox1.Text;
            }
            if (textBox2.Text != "关闭窗口自动保存")
            {
                Properties.Settings.Default.SecretKey = textBox2.Text;
            }
            Properties.Settings.Default.Save();

            if (textBox1.Text != "关闭窗口自动保存" || textBox2.Text != "关闭窗口自动保存" || textBox1.Text != "" || textBox2.Text != "" || textBox1.Text != " " || textBox2.Text != " ")
            {
                MainWindow mainWin = new MainWindow();
                mainWin.switch3.IsChecked = false;
            }
            MainWindow.changeStatus = false;
        }
    }
}
