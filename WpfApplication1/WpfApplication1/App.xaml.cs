using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private FirstWindow firstWindow;
        private MainWindow mainWindow;
        private DatabaseManager databaseManager;

        public App()
        {
            databaseManager = new DatabaseManager();
            
            firstWindow = new FirstWindow();
            firstWindow.message += receiveMessage;
            firstWindow.updateLast(databaseManager.checkProjects());

            mainWindow = new MainWindow();
            mainWindow.message += receiveMessage;

            App.Current.MainWindow = firstWindow;
            firstWindow.Show();
        }

        private void receiveMessage(object sender, MessageEvent e){
            if (e.type.Contains("wpf"))
            {
                if (e.value.Contains("mainwindow"))
                {
                    firstWindow.Hide();
                    mainWindow.Show();
                }
                else if (e.value.Contains("firstwindow"))
                {
                    mainWindow.Hide();
                    firstWindow.updateLast(databaseManager.getLastProjects());
                    firstWindow.Show();
                }
            }
            else if (e.type.Contains("exit"))
            {
                Application.Current.Shutdown();
            }
            else if (e.type.Contains("database_add"))
            {
                databaseManager.insertFile(e.value.ToString());
            }
            else if (e.type.Contains("open"))
            {
                Console.WriteLine(e.value.ToString());
                firstWindow.Hide();
                mainWindow.openFile(e.value.ToString());
                mainWindow.Show();
            }
        }

    }
}
