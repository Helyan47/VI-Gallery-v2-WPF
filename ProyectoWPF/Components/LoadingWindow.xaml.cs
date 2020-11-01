using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProyectoWPF.Components {
    /// <summary>
    /// Lógica de interacción para LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window {

        DispatcherTimer timer;
        public const long MULTI_FOLDER_MODE = 0L;
        public const long LOAD_PROFILE_MODE = 1L;
        private string multiFolderPath = "";
        private string multiFolderName = "";
        private int maxFolders = 0;
        private int maxFiles = 0;
        private int allData = 0;
        private int actualNumFolder = 0;
        private int actualNumFile = 0;
        private int actualNumAll = 0;
        int[][] _filesFoldersCount = null;
        int cont = 0;

        private long mode = -1L;

        public LoadingWindow() {
            InitializeComponent();
            //timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 1);
            //timer.Tick += tick;
        }

        private void tick(object sender, EventArgs e) {
            if(cont == 0) {
                LoadingPercentage.Content = "Loading.";
                cont++;
            }else if(cont == 1) {
                LoadingPercentage.Content = "Loading..";
                cont++;
            } else if (cont == 2) {
                LoadingPercentage.Content = "Loading...";
                cont=0;
            }
        }

        public void setMode(long mode,string multiFolderPath) {
            ClearData();
            if(mode == MULTI_FOLDER_MODE) {
                this.mode = mode;
                this.multiFolderPath = multiFolderPath;
                //timer.Start();
                if (!multiFolderPath.Equals("")) {
                    StartLoadingMultiFolder();

                } else {
                    ClearData();
                }
                
            }else if(mode == LOAD_PROFILE_MODE) {

            }
        }

        private void StartLoadingMultiFolder() {
            try {
                string[] folders = OrderClass.orderArrayOfString(Directory.GetDirectories(multiFolderPath));
                multiFolderName = System.IO.Path.GetFileNameWithoutExtension(multiFolderPath);
                if (folders != null && folders.Length > 0) {
                    ThreadFolder[] threads = new ThreadFolder[folders.Length];
                    for (int i = 0; i < folders.Length; i++) {
                        threads[i] = new ThreadFolder(folders[i]);
                    }

                    for (int i = 0; i < threads.Length; i++) {
                        threads[i].StartAndJoin();
                    }
                    _filesFoldersCount = new int[threads.Length][];
                    for (int i = 0; i < threads.Length; i++) {
                        _filesFoldersCount[i] = new int[2];
                        _filesFoldersCount[i][0] = threads[i].getFolders();
                        _filesFoldersCount[i][1] = threads[i].getFiles();
                    }
                    maxFiles = 0;
                    maxFolders = 0;
                    allData = 0;
                    for (int i = 0; i < _filesFoldersCount.Length; i++) {
                        maxFolders += _filesFoldersCount[i][0];
                        maxFiles += _filesFoldersCount[i][1];
                        allData = maxFolders + maxFiles;
                    }
                    calculateTotalPercentage();
                    InformationText.Content = "Nothing";
                    ProgressText.Content = "( 0 / " + allData + ")";
                }
            }catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public void notifyNewFolder(string folder) {
            try {
                this.Dispatcher.BeginInvoke(new Action(() => {
                    if ((actualNumFolder + 1) <= maxFolders) {
                        actualNumFolder++;
                        calculateTotalPercentage();
                        InformationText.Content = "Creating folder " + folder + " (" + actualNumFolder + " / " + maxFolders + ")";
                        actualNumAll = actualNumFolder + actualNumFile;
                        ProgressText.Content = "(" + actualNumFolder + " / " + allData + ")";
                    }
                }));
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public void notifyNewFile(string file) {
            this.Dispatcher.BeginInvoke(new Action(() => {
                if ((actualNumFile + 1) <= maxFiles) {
                    actualNumFile++;
                    calculateTotalPercentage();
                    InformationText.Content = "Creating file " + file + " (" + actualNumFile + " / " + maxFiles + ")";
                    ProgressText.Content = "(" + actualNumFile + " / " + allData + ")";
                }
            }));
        }

        private void calculateTotalPercentage() {
            try {
                this.Dispatcher.BeginInvoke(new Action(() => {
                    double totalPercentage = ((((double)actualNumFolder + (double)actualNumFile) / (double)allData) * 100);

                    string value = LoadingPercentage.Content.ToString();
                    try {
                        value = String.Format("{0:0.00}", totalPercentage);
                    } catch (Exception e) {

                    }
                    LoadingPercentage.Content = value + "%";
                }));
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }

        public void ClearData() {
            this.mode = -1L;
            multiFolderPath = "";
            multiFolderName = "";
            maxFolders = 0;
            maxFiles = 0;
            allData = 0;
            actualNumFolder = 0;
            actualNumFile = 0;
            actualNumAll = 0;
            _filesFoldersCount = null;
            LoadingPercentage.Content = "";
            InformationText.Content = "";
            ProgressText.Content = "";
            progressBar.Width = new GridLength(0, GridUnitType.Star);
            fullBar.Width = new GridLength(100, GridUnitType.Star);
        }
    }

    public class ThreadFolder {
        private Thread thread;
        private string folder;
        private int numFolders;
        private int numFiles;

        public ThreadFolder(string folder) {
            numFolders = 0;
            numFiles = 0;
            this.folder = folder;
            thread = new Thread(() => {
                numFolders = Directory.GetDirectories(this.folder, "*", SearchOption.AllDirectories).Length;
                numFiles = Directory.GetFiles(this.folder, "*", SearchOption.AllDirectories).Length;
            });
        }

        public void StartAndJoin() {
            thread.Start();
            thread.Join();
        }

        public int getFolders() {
            return numFolders;
        }

        public int getFiles() {
            return numFiles;
        }
    }
}
