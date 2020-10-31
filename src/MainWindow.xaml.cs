using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using PL_Resolution.Logic.Models;
using PL_Resolution.Logic.Services;

namespace PL_Resolution
{
    public partial class MainWindow
    {
        public MainWindow()
        { 
            InitializeComponent();
        }

        private void LoadClausesFromFile(object sender, RoutedEventArgs e)
        {
            var dll = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var filesToLoadDir = dll.Directory.Parent.Parent.Parent.Parent.FullName + "\\SampleInputFiles";

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = filesToLoadDir,
                Title = "Załaduj plik z danymi w określonym formacie",
                FilterIndex = 2,
                RestoreDirectory = false,
                CheckFileExists = true,
                CheckPathExists = true,
            };

            var result = ofd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var fileLines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                string content = "";
                try
                {
                    var clauses = Parser.Parse(fileLines);
                    var solver = new Solver();
                    var resolution = solver.FindResolution(clauses);
                    ResultLabel.Content = resolution.result ? "Znaleziono rozwiązanie" : "Brak rozwiązania";
                    content = resolution.log;
                }
                catch (ParseException parseException)
                {
                    content = parseException.Message;
                }

                // https://stackoverflow.com/questions/7861699/cannot-see-underscore-in-wpf-content
                LogLabel.Content = content.Replace("_", "__");
            }
        }
    }
}