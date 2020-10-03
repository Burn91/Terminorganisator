using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;
using System.IO;
using Path = System.IO.Path;

namespace Terminorganisator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary
    /// >
    /// 





    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            Dp_Datum.SelectedDate = DateTime.Now;

            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    Lsbx_Stunden.Items.Add("0" + i);
                }
                else
                {
                    Lsbx_Stunden.Items.Add(i);
                }

            }
            Lsbx_Stunden.SelectedIndex = 0;

            for (int i = 0; i < 59; i++)
            {
                if (i < 10)
                {
                    Lsbx_Minuten.Items.Add("0" + i);
                }
                else
                {
                    Lsbx_Minuten.Items.Add(i);
                }

            }

            Lsbx_Minuten.SelectedIndex = 0;

            Txb_Termin.Foreground = Brushes.Red;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            StreamReader sr = new StreamReader(docPath + "\\test.txt");

            string einlesen = sr.ReadToEnd().ToString();
            sr.Close();

            int anzahl = einlesen.Split('\n').Length;
            

            for (int i = 0; i < anzahl-1; i++)
            {
                string termine = einlesen.Split('\n')[i].ToString();
                string Uhrzeit = termine.Split(';')[0].ToString();
                string Datum = termine.Split(';')[1].ToString();
                string Termin = termine.Split(';')[2].ToString();
                Termin = Termin.Split('\r')[0].ToString();

                lvTermine.Items.Add(
                                    new
                                          {
                                            Uhrzeit = Uhrzeit ,
                                            Datum = Datum,
                                            Termin = Termin,
              });

            }
            

           


            timer.Start();
            Title = "Terminorganisator";





        }

        void timer_Tick(object sender,EventArgs e)
        {

            for (int i = 0; i < lvTermine.Items.Count; i++)
            {
                string s = lvTermine.Items[i].ToString();
                string[] liste = s.Split(',');

                string uhrzeit = liste[0];
                uhrzeit = uhrzeit.Remove(0, 12);
                uhrzeit = uhrzeit.Trim(' ');

                string Datum = liste[1];
                Datum = Datum.Remove(0, 8);
                Datum = Datum.Trim(' ');


                string datum1 = DateTime.Today.ToString("d");
                string uhrzeit1 = DateTime.Now.ToString("t");

                if (uhrzeit == uhrzeit1 && datum1 == Datum)
                {
                    string termin = liste[2];
                    termin = termin.Remove(0, 9);
                    termin = termin.TrimEnd('}', ' ');
                    termin = termin.TrimStart(' ');
                    MessageBox.Show(uhrzeit + " - " + Datum + "\n" + termin, "Termin Erinnerung", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    lvTermine.Items.Remove(lvTermine.Items[i]);                }
            }
           

          

           
        }



        private void Btm_Termin_Hinzufügen_Click(object sender, RoutedEventArgs e)
        {

            
            string Uhrzeit = Lsbx_Stunden.SelectedItem.ToString() + ":" + Lsbx_Minuten.SelectedItem.ToString();
            string Datum = Dp_Datum.SelectedDate.ToString().Split(' ')[0].ToString();
            string Termin = Txb_Termin.Text.ToString();
            Termin = Termin.Replace(';', '|');

            lvTermine.Items.Add(
              new
              {
                  Uhrzeit = Uhrzeit,
                  Datum = Datum,
                  Termin = Termin,
              });



        }



        private void Btn_Termin_Löschen_Click(object sender, RoutedEventArgs e)
        {
            lvTermine.Items.Remove(lvTermine.SelectedItem);
        }

        private void lvTermine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Txb_Termin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Txb_Termin.Text == "Hier Termin eingeben...")
            {
                Txb_Termin.Text = "";
                Txb_Termin.Foreground = Brushes.Black;
            }
        }

      
        private void Mainwindow_closed(object sender, EventArgs e)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            StreamWriter outputfile = new StreamWriter(Path.Combine(docPath, "test.txt"));

            for (int i = 0; i < lvTermine.Items.Count; i++)
            {
                string s = lvTermine.Items[i].ToString();
                string[] liste = s.Split(',');

                string uhrzeit = liste[0];
                uhrzeit = uhrzeit.Remove(0, 12);
                uhrzeit = uhrzeit.Trim(' ');

                string Datum = liste[1];
                Datum = Datum.Remove(0, 8);
                Datum = Datum.Trim(' ');

                string termin = liste[2];
                termin = termin.Remove(0, 9);
                termin = termin.Trim('}', ' ');

                outputfile.WriteLine(uhrzeit + ";" + Datum + ";" + termin);

            }

            outputfile.Close();
        }

        private void Btm_AlleLöschen_Click(object sender, RoutedEventArgs e)
        {
          
            lvTermine.Items.Clear();
        }

        
    }
}



