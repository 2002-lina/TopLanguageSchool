using Microsoft.Win32;
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
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using MessageBox =  System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using System.Data.Entity.Migrations;
using System.Text.RegularExpressions;

namespace LanguageSchool
{
    /// <summary>
    /// Логика взаимодействия для Yslugi.xaml
    /// </summary>
    public partial class Yslugi : Page
    {
        List<Service> ServiceList = Base.ZE.Service.ToList();

        public Yslugi()
        {
            InitializeComponent();
            DGServises.ItemsSource = ServiceList;

            FIO.ItemsSource = Base.ZE.Client.ToList();
            FIO.SelectedValuePath = "ID";
            FIO.DisplayMemberPath = "Fio";
        }
        int i = -1;
        int indexChange;

        private void StackPanel_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                StackPanel SP = (StackPanel)sender;
                Service SE = ServiceList[i];
                if (SE.Discount != 0)
                {
                    SP.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));
                }
            }
        }

        private void MediaElement_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                i++;
                MediaElement ME = (MediaElement)sender;
                Service SE = ServiceList[i];
                Uri U = new Uri(SE.MainImagePath, UriKind.RelativeOrAbsolute);
                ME.Source = U;
            }

        }

        private void title_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                TextBlock TB = (TextBlock)sender;
                Service SE = ServiceList[i];
                TB.Text = SE.Title;
            }
        }

        private void skidka_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                TextBlock TB = (TextBlock)sender;
                Service SE = ServiceList[i];
                if (SE.Discount == 0)
                {

                    int old_cost = Convert.ToInt32(SE.Cost);
                    int time = Convert.ToInt32(SE.DurationInSeconds / 60);
                    TB.Text = Convert.ToString(" " + old_cost + " рублей за " + time + " минут");
                }
                else
                {
                    TB.Text = Convert.ToString("* скидка " + SE.Discount * 100 + "%");
                }

            }
        }

        private void DGRed_Initialized(object sender, EventArgs e)
        {
            Button BtnRed = (Button)sender;
            if (BtnRed != null)
            {
                BtnRed.Uid = Convert.ToString(i);
            }
        }

        private void newcost_Initialized(object sender, EventArgs e)
        {
            if (i < ServiceList.Count)
            {
                TextBlock TB = (TextBlock)sender;
                Service SE = ServiceList[i];
                int discount = Convert.ToInt32(SE.Discount * 100);
                int cost = Convert.ToInt32(SE.Cost);
                int cost_dis = cost - (cost / 100 * discount);
                int time = Convert.ToInt32(SE.DurationInSeconds / 60);
                if (SE.Discount == 0)
                {
                    TB.Visibility = Visibility.Collapsed;
                }
                else
                {

                    TB.Text = Convert.ToString(" " + cost_dis + " рублей за " + time + " минут");
                }
            }

        }

        private void DGRed_Click(object sender, RoutedEventArgs e)
        {
            Button BtnRed = (Button)sender;
            int ind = Convert.ToInt32(BtnRed.Uid);
            Service S = ServiceList[ind];
            DGServises.Visibility = Visibility.Collapsed;
            Forms.Visibility = Visibility.Visible;
            Title_ysl.Visibility = Visibility.Collapsed;
            Zapic.Visibility = Visibility.Collapsed;
            RDID.Text = Convert.ToString(S.ID);
            RDtitle.Text = Convert.ToString(S.Title);
            RDcost.Text = Convert.ToInt32(S.Cost) + "";
            RDdlit.Text = Convert.ToInt32(S.DurationInSeconds / 60) + "";
            RDopis.Text = Convert.ToString(S.Description);
            RDskid.Text = Convert.ToString(S.Discount * 100);
            RDimg.Text = Convert.ToString(S.MainImagePath);

        }

        private void cost_Initialized(object sender, EventArgs e)
        {

            TextBlock TB = (TextBlock)sender;
            Service SE = ServiceList[i];
            if (SE.Discount == 0)
            {
                TB.Text = " ";
            }
            else
            {
                int old_cost = Convert.ToInt32(SE.Cost);
                TB.TextDecorations = TextDecorations.Strikethrough;
                TB.Text = Convert.ToString(old_cost);
            }
        }
        private void DGDel_Initialized(object sender, EventArgs e)
        {
            Button BtnDel = (Button)sender;
            if (BtnDel != null)
            {
                BtnDel.Uid = Convert.ToString(i);
            }
        }
        private void DGDel_Click(object sender, RoutedEventArgs e)
        {
            Button BtnDel = (Button)sender;
            int ind = Convert.ToInt32(BtnDel.Uid);
            DialogResult dialogResult = (DialogResult)MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", (MessageBoxButton)MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Service S = ServiceList[ind];
                Base.ZE.Service.Remove(S);
                MessageBox.Show("Запись удалена");
                Base.ZE.SaveChanges();
                Frames.FR.Navigate(new Yslugi());
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Запись не  удалена");
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DGServises.Visibility = Visibility.Visible;
            Forms.Visibility = Visibility.Collapsed;
            Frames.FR.Navigate(new Yslugi());
        }

        private void Img_Initialized(object sender, EventArgs e)
        {
            Button BtnDel = (Button)sender;
            if (BtnDel != null)
            {
                BtnDel.Uid = Convert.ToString(i);
            }
        }
        private void Img_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string path = OFD.FileName;
            if (path != " ")
            {
                int c = path.IndexOf('У');
                string len = path.Substring(c);
                RDimg.Text = len.ToString();
            }

        }

        private void SVIzm_Initialized(object sender, EventArgs e)
        {
            Button SVIzm = (Button)sender;
            if (SVIzm != null)
            {
                SVIzm.Uid = Convert.ToString(i);
            }
        }

        private void SVIzm_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(ZPdlit.Text) > 14400 || Convert.ToInt32(ZPdlit.Text) < 0)
            {
                MessageBox.Show("Время не должно быть отрицательным и не должно превышать 4 часов (14400сек)");
            }
            else if (Convert.ToInt32(ZPdlit.Text) < 14400 || Convert.ToInt32(ZPdlit.Text) > 0)
            {
                double skidka = Convert.ToDouble(RDskid.Text) / 100;
                int time = Convert.ToInt32(RDdlit.Text) * 60;
                DialogResult dialogResult = (DialogResult)MessageBox.Show("Вы действительно хотите внести изменения?", "Сохранение изменений", (MessageBoxButton)MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Service ServiceObject = new Service() { ID = Convert.ToInt32(RDID.Text), Title = RDtitle.Text, Cost = Convert.ToInt32(RDcost.Text), DurationInSeconds = time, Description = RDopis.Text, Discount = skidka, MainImagePath = RDimg.Text };
                    Base.ZE.Service.AddOrUpdate(ServiceObject);
                    Base.ZE.SaveChanges();
                    MessageBox.Show("Изменения сохранены");
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Изменения не сохранены");
                }
            }


        }

        private void Zapic_Initialized(object sender, EventArgs e)
        {
            Button Zapic = (Button)sender;
            if (Zapic != null)
            {
                Zapic.Uid = Convert.ToString(i);
            }
        }

        private void Zapic_Click(object sender, RoutedEventArgs e)
        {
            Zapic.Visibility = Visibility.Collapsed;
            DGServises.Visibility = Visibility.Collapsed;
            Forms.Visibility = Visibility.Collapsed;
            Forms_zap.Visibility = Visibility.Visible;
            Title_ysl.Visibility = Visibility.Collapsed;
        }

        private void ZPImg_Initialized(object sender, EventArgs e)
        {
            Button BtnDel = (Button)sender;
            if (BtnDel != null)
            {
                BtnDel.Uid = Convert.ToString(i);
            }
        }

        private void ZPImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string path = OFD.FileName;
            if (path != " ")
            {
                int c = path.IndexOf('У');
                string len = path.Substring(c);
                ZPimg.Text = len.ToString();
            }
        }

        private void ZPSVIzm_Initialized(object sender, EventArgs e)
        {
            Button ZPSVIzm = (Button)sender;
            if (ZPSVIzm != null)
            {
                ZPSVIzm.Uid = Convert.ToString(i);
            }
        }

        private void ZPSVIzm_Click(object sender, RoutedEventArgs e)
        {

            Service SE = ServiceList[i];
            if (SE.Title == ZPtitle.Text)
            {
                MessageBox.Show("Такое название уже существует! Придумайте новое");
            }

            if (Convert.ToInt32(ZPdlit.Text) > 14400 || Convert.ToInt32(ZPdlit.Text) < 0)
            {
                MessageBox.Show("Время не должно быть отрицательным и не должно превышать 4 часов (14400сек)");
            }
            else if (Convert.ToInt32(ZPdlit.Text) < 14400 || Convert.ToInt32(ZPdlit.Text) > 0)
            {
                double skidka = Convert.ToDouble(ZPskid.Text) / 100;
                int time = Convert.ToInt32(ZPdlit.Text) * 60;
                DialogResult dialogResult = (DialogResult)MessageBox.Show("Вы действительно хотите добавить новую услугу?", "Добавление услуги", (MessageBoxButton)MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Service ServiceObject = new Service() { Title = ZPtitle.Text, Cost = Convert.ToInt32(ZPcost.Text), DurationInSeconds = time, Description = ZPopis.Text, Discount = skidka, MainImagePath = ZPimg.Text };
                    Base.ZE.Service.Add(ServiceObject);
                    Base.ZE.SaveChanges();
                    MessageBox.Show("Услуга добавлена");
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Услуга не добавлена");
                }
            }


        }

        private void ZPBack_Click(object sender, RoutedEventArgs e)
        {

            Frames.FR.Navigate(new Yslugi());
        }

        private void DGDobav_Initialized(object sender, EventArgs e)
        {
            Button DGDobav = (Button)sender;
            if (DGDobav != null)
            {
                DGDobav.Uid = Convert.ToString(i);
            }
        }
       
        private void DGDobav_Click(object sender, RoutedEventArgs e)
        {
            Button DGDobav = (Button)sender;
            int ind = Convert.ToInt32(DGDobav.Uid);
            indexChange = Convert.ToInt32(DGDobav.Uid);
            Service S = ServiceList[ind]; 
            Forms_zap_yslugi.Visibility = Visibility.Visible;
            Zapic.Visibility = Visibility.Collapsed;
            DGServises.Visibility = Visibility.Collapsed;
            Forms.Visibility = Visibility.Collapsed;
            Forms_zap.Visibility = Visibility.Collapsed;
            Title_ysl.Visibility = Visibility.Collapsed;
            ZP_ysl_title.Text = Convert.ToString(S.Title);
            ZP_ysl_dlit.Text = Convert.ToInt32(S.DurationInSeconds / 60) + "" + " мин";
        }

     

        private void ZP_uslug_Initialized(object sender, EventArgs e)
        {
            Button ZP_uslug = (Button)sender;
            if (ZP_uslug != null)
            {
                ZP_uslug.Uid = Convert.ToString(i);
            }
        }
        DateTime Dt;
        private void ZP_ysl_time_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex r1 = new Regex("[0-1][0-9]:[0-5][0-9]");
            Regex r2 = new Regex("2[0-3:][0-5][0-9]");
            string s = "";

            if ((r1.IsMatch(ZP_ysl_time.Text) || r2.IsMatch(ZP_ysl_time.Text)) && ZP_ysl_time.Text.Length == 5)
            {

                TimeSpan TS = TimeSpan.Parse(ZP_ysl_time.Text);
                Dt = Convert.ToDateTime(Data.SelectedDate);
                Dt = Dt.Add(TS);
                if (Dt < DateTime.Now)
                {
                    MessageBox.Show("На выбранную дату вы записаться не можете!");
                    ZP_uslug.IsEnabled = false;
                }

            }

            else
            {
                if (ZP_ysl_time.Text.Length >= 5)
                {
                    MessageBox.Show("Время указзано неверно");
                    ZP_uslug.IsEnabled = false;
                }


            }

        }
 
        private void ZP_uslug_Click(object sender, RoutedEventArgs e)
        {
            int ind = indexChange;
            Service SE = ServiceList[ind];
            int index = FIO.SelectedIndex + 1;
            DialogResult dialogResult = (DialogResult)MessageBox.Show("Вы действительно хотите записаться на  услугу?", "Запись на  услугу", (MessageBoxButton)MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ClientService ClientServiceObject = new ClientService() { ClientID = index, ServiceID = SE.ID, StartTime = Dt };
                Base.ZE.ClientService.Add(ClientServiceObject);
                Base.ZE.SaveChanges();
                MessageBox.Show("Запись прошла успешна");
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Вы не записались");
            }



        }

        private void ZP_yslug_Back_Click(object sender, RoutedEventArgs e)
        {
            Frames.FR.Navigate(new Yslugi());
        }
    }
}


 


            
