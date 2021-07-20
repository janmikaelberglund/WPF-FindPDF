using IOServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using IOServices.Services;
using Ookii.Dialogs;
using Ookii.Dialogs.Wpf;
using IOServices;

namespace WPF_FindPDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<PdfModel> PdfList = new();
        public static string path { get; set; }

        VistaFolderBrowserDialog Dialog = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            path = PathInput.Text;
        }

        [STAThread]
        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            SaveResultLabel.Content = "";
            Dialog.Description = "Öppna projektmapp";
            var result = Dialog.ShowDialog();

            if ((bool)result)
            {
                PathInput.Text = Dialog.SelectedPath;
            }
            if (!Directory.Exists(Dialog.SelectedPath + "\\Beräkningar\\"))
            {
                MessageBox.Show($"Mappen Beräkningar hittas inte i {Dialog.SelectedPath}", "Ehe ehe Mojje!");
                PathInput.Text = "";
                return;
            }
        }

        private void Button_Run_Click(object sender, RoutedEventArgs e)
        {
            Output_Listbox.Items.Clear();

            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            
            PdfList = PdfService.GetPdfList(path + "\\Beräkningar\\");

            int count = 0;
            foreach (var pdfFile in PdfList)
            {
                string filename = pdfFile.FileName[0..^4];
                Output_Listbox.Items.Add($"{filename},{pdfFile.NumberOfPages},{count}");
                count += pdfFile.NumberOfPages;
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(path) || PdfList == null)
            {
                SaveResultLabel.Foreground = new SolidColorBrush(Colors.Red);
                SaveResultLabel.Content = "Ett fel har inträffat.";
                return;
            }
            try
            {
                using (StreamWriter streamWriter = new(System.IO.Path.Combine(path + "\\Beräkningar.csv")))
                {
                    int count = 1;
                    foreach (var pdfFile in PdfList)
                    {
                        string filename = pdfFile.FileName[0..^4];
                        streamWriter.WriteLine($"{filename},{pdfFile.NumberOfPages},{pdfFile.FullPath},{count}");
                        count += pdfFile.NumberOfPages;
                    }
                }
                MessageBox.Show("Filen sparad!");
            }
            catch (Exception)
            {
                SaveResultLabel.Foreground = new SolidColorBrush(Colors.Red);
                SaveResultLabel.Content = "Ett fel har inträffat.";
            }
            
        }
    }
}
