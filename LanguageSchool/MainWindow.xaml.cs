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

namespace LanguageSchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Path;
        public MainWindow()
        {
            InitializeComponent();
            Base.ZE = new ZheleznovaEntities();
            Frames.FR = frm;
            Frames.FR.Navigate(new Yslugi());
            BitmapImage BMI = new BitmapImage();
            BMI.BeginInit();
            Path = @"img\school_logo.png";
            BMI.UriSource = new Uri(Path, UriKind.RelativeOrAbsolute);
            BMI.EndInit();
            logotip.Source = BMI;
            logotip.Stretch = Stretch.UniformToFill;

        }
    }
}
