using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Image_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //instance data
        DirectoryInfo imageDir;
        int currentPic = 0;

        public MainWindow()
        {
            InitializeComponent();
            setInitialPicture();
        }

        public void GetImages()
        {
            var images = from x in imageDir.GetFiles() where x.Extension == ".jpg" || x.Extension == ".png" select x;
            foreach (var ele in images)
            {
                ImageList.Items.Add(new BitmapImage(new Uri(ele.FullName)));
            }
        }

        private void setInitialPicture() {
            BigImage.Source = new BitmapImage(new Uri("Assets/patchy.jpg", UriKind.Relative));
        }

        private void SetDirectory(object sender, RoutedEventArgs e)
        {
            var myOpenFileDialog = new CommonOpenFileDialog();
            myOpenFileDialog.IsFolderPicker = true;
            if (myOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                imageDir = new DirectoryInfo(myOpenFileDialog.FileName);

                if (ImageList.Items.Count > 0)
                {
                    currentPic = 0;
                    ImageList.Items.Clear();
                    setInitialPicture();
                    ImageList.Items.Refresh();
                }

                GetImages();
            }
        }

        private void DisplayImageInMain(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine(sender.GetType());
            //Console.Write(BigImage.GetType());
            Image tempPic = (Image)sender;

            BigImage.Source = tempPic.Source;

            currentPic = ImageList.Items.IndexOf(((Image)sender).Source);
            Console.WriteLine(currentPic);

        }

        private void Shutdown(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigate using the sidebar or with the numpad keys.\nMade in 2017 by Lunarolm.");
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad2:
                    if (currentPic < (ImageList.Items.Count -1 ))
                    {
                        currentPic++;
                        SetImage(e);
                    }
                    break;
                case Key.NumPad8:
                    if (currentPic > 0)
                    {
                        currentPic--;
                        SetImage(e);
                    }
                    else
                    {
                        Console.WriteLine("THE INDEX IS TOO LOW");
                    }
                    break;
            }
        }

        public void SetImage(KeyEventArgs e)
        {
            var newImage = (BitmapImage)ImageList.Items.GetItemAt(currentPic);
            BigImage.Source = newImage;
            Console.WriteLine(currentPic);
            e.Handled = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.NumPad2 || e.Key == Key.NumPad8)
            {
                e.Handled = true;
            }
        }

    }
}
