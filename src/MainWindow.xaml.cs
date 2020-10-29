using System.Collections.Generic;
using System.IO;
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
            // TestMethod();
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
                var fileLines = File.ReadAllLines(ofd.FileName);
                try
                {
                    var parseResult = Parser.Parse(fileLines);
                    var solver = new Solver(parseResult.indexToName, true);
                    var resolution = solver.FindResolution(parseResult.clauses);
                    ResultLabel.Content = resolution.result;
                    LogLabel.Content = resolution.log;
                }
                catch (ParseException parseException)
                {
                    LogLabel.Content = parseException.Message;
                }
            }
        }
    }
}