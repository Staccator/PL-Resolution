using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Win32;

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
            const int fileShapeSize = 5;

            if (result.HasValue && result.Value)
            {
                var file = File.ReadAllText(ofd.FileName);
                var split = file.Split(new[] {' ', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}