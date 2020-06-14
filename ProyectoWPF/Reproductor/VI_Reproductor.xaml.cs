using ProyectoWPF.Data.Online;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Vlc.DotNet.Core;

namespace ProyectoWPF.Reproductor {
    /// <summary>
    /// Lógica de interacción para VI_Reproductor.xaml
    /// </summary>
    public partial class VI_Reproductor : UserControl {

        private object[] lista;
        private long[] _capitulos = null;
        private long[] _peliculas = null;
        private object[] names;
        public int currentVideoPosition { get; set; }
        public static bool isOnline = false;

        public static int volumen = 100;
        object actualVideo { get; set; }

        int cont = 0;

        int segundos, minutos, horas = 0;
        bool isClicking { get; set; }
        bool hasHours { get; set; }
        bool isExpanded { get; set; }
        DispatcherTimer dp { get; set; }

        private VIGallery main = null;

        public Window MainWindow;
        public VideoPlayerProperties videoPlayerProperties { get; set; }

        public VI_Reproductor() {
            InitializeComponent();
            try {
                videoPlayerProperties = new VideoPlayerProperties(this, control);
                isClicking = false;
                isExpanded = false;
                Volumen.Value = volumen;
                currentVideoPosition = 0;
                var currentAssembly = Assembly.GetEntryAssembly();
                var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
                var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
                Console.WriteLine(libDirectory.FullName);
                dp = new DispatcherTimer();
                dp.Interval = new TimeSpan(0, 0, 1);
                dp.Tick += tick;
                control.SourceProvider.CreatePlayer(libDirectory);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void setOnline(bool online) {
            isOnline = online;
        }

        public void setVIGallery(VIGallery vi) {
            main = vi;
        }

        public Window getWindow() {
            return MainWindow;
        }

        private void setTimeLabel() {
            try {
                string seconds;
                string minutes;
                string hours;
                long duration = control.SourceProvider.MediaPlayer.Time;
                segundos = (int)(duration / 1000);
                minutos = (int)segundos / 60;
                horas = (int)minutos / 60;
                minutos = (int)minutos % 60;
                segundos = (int)segundos % 60;

                if (horas == 0) {
                    seconds = segundos + "";
                    minutes = minutos + "";
                    currentTime.Content = minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
                } else {
                    seconds = segundos + "";
                    minutes = minutos + "";
                    hours = horas + "";
                    currentTime.Content = hours.PadLeft(2, '0') + ":" + minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void tick(object sender, EventArgs e) {
            try {
                if (control.SourceProvider.MediaPlayer.IsPlaying()) {

                    long duration;
                    string seconds;
                    string minutes;
                    string hours;
                    if (cont == 0) {
                        timeLine.Maximum = control.SourceProvider.MediaPlayer.Length;
                        cont++;
                        duration = control.SourceProvider.MediaPlayer.Length;
                        segundos = (int)(duration / 1000);
                        minutos = (int)segundos / 60;
                        horas = (int)minutos / 60;
                        minutos = (int)minutos % 60;
                        segundos = (int)segundos % 60;

                        if (horas == 0) {
                            seconds = segundos + "";
                            minutes = minutos + "";
                            timeDuration.Content = minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
                            hasHours = false;
                        } else {
                            seconds = segundos + "";
                            minutes = minutos + "";
                            hours = horas + "";
                            timeDuration.Content = hours.PadLeft(2, '0') + ":" + minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
                            hasHours = true;
                        }
                        duration = control.SourceProvider.MediaPlayer.Time;
                        segundos = (int)(duration / 1000);
                        minutos = (int)segundos / 60;
                        horas = (int)minutos / 60;
                        minutos = (int)minutos % 60;
                        segundos = (int)segundos % 60;
                    }
                    segundos++;
                    if (segundos >= 60) {
                        segundos = 0;
                        minutos++;
                        if (minutos >= 60) {
                            minutos = 0;
                            horas++;
                        }

                    }
                    if (!hasHours) {
                        seconds = segundos + "";
                        minutes = minutos + "";

                        currentTime.Content = minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
                    } else {
                        seconds = segundos + "";
                        minutes = minutos + "";
                        hours = horas + "";

                        currentTime.Content = hours.PadLeft(2, '0') + ":" + minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
                    }

                    timeLine.Value = control.SourceProvider.MediaPlayer.Time;
                    if (timeLine.Value >= control.SourceProvider.MediaPlayer.Length - 1000) {
                        nextVideo();
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }


        public void setLista(object[] args) {
            try {
                if (args != null & args.Length != 0) {
                    lista = args;
                    videoTitle.Content = names[currentVideoPosition];
                    setVideo(lista[0]);
                }
                control.Focus();
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void setListaCapitulos(object[] args, long[] capitulos, int position) {
            try {
                if (args != null & capitulos != null & args.Length != 0) {
                    lista = args;
                    _capitulos = capitulos;
                    currentVideoPosition = position;
                    videoTitle.Content = names[currentVideoPosition];
                    setVideo(lista[position]);
                    control.Focus();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void setListaCapitulos(object[] args, long[] capitulos) {
            try {
                if (args != null & capitulos != null & args.Length != 0) {
                    lista = args;
                    _capitulos = capitulos;
                    currentVideoPosition = 0;
                    videoTitle.Content = names[currentVideoPosition];
                    setVideo(lista[0]);
                    control.Focus();
                }
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }
        public void setListaPeliculas(object[] args, long[] peliculas, int position) {
            try {
                if (args != null & peliculas != null & args.Length != 0) {
                    lista = args;
                    _peliculas = peliculas;
                    currentVideoPosition = position;
                    videoTitle.Content = names[currentVideoPosition];
                    setVideo(lista[position]);
                    control.Focus();
                }
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void setListaPeliculas(object[] args, long[] peliculas) {
            try {
                if (args != null & peliculas != null & args.Length != 0) {
                    lista = args;
                    _peliculas = peliculas;
                    currentVideoPosition = 0;
                    videoTitle.Content = names[currentVideoPosition];
                    setVideo(lista[0]);
                    control.Focus();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void setLista(object[] args, int position) {
            try {
                if (args != null & args.Length != 0) {
                    lista = args;
                    currentVideoPosition = position;
                    videoTitle.Content = names[currentVideoPosition];
                    setVideo(lista[position]);
                    control.Focus();
                }
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }

        }


        public void setVolume(int volume) {
            try {
                control.SourceProvider.MediaPlayer.Audio.Volume = volume;
                volumen = volume;
            } catch (Exception e) {

            }
        }


        public void playPause() {
            if (control.SourceProvider.MediaPlayer.IsPlaying()) {
                control.SourceProvider.MediaPlayer.Pause();
                viewBoxPause.Visibility = Visibility.Hidden;
                viewBoxPlay.Visibility = Visibility.Visible;
            } else {
                control.SourceProvider.MediaPlayer.Play();
                viewBoxPause.Visibility = Visibility.Visible;
                viewBoxPlay.Visibility = Visibility.Hidden;
            }
        }


        public void timeChanged(long time) {
            control.SourceProvider.MediaPlayer.Time = time;
        }


        public void setVideo(object file) {
            try {
                if (file is FileInfo) {
                    FileInfo aux = (FileInfo)file;
                    control.SourceProvider.MediaPlayer.Play(aux);
                    actualVideo = file;
                    videoTitle.Content = aux.Name;
                } else if (file is Uri) {
                    control.SourceProvider.MediaPlayer.Play((Uri)file);
                    actualVideo = file;
                } else if (file is Stream) {
                    control.SourceProvider.MediaPlayer.Play((Stream)file);
                    actualVideo = file;
                } else {
                    MessageBox.Show("Tipo de archivo no soportado");
                }
                if (isOnline) {
                    if (_capitulos != null) {
                        try {
                            ConexionServer.increaseNumVisitasCap(_capitulos[currentVideoPosition]);
                            long time = ConexionServer.getTimeCapitulo(_capitulos[currentVideoPosition]);
                            control.SourceProvider.MediaPlayer.Time = time;
                        } catch (Exception e) {
                            control.SourceProvider.MediaPlayer.Time = 0;
                        }
                    } else if (_peliculas != null) {
                        try {
                            ConexionServer.increaseNumVisitasPelicula(_peliculas[0]);
                            long time = ConexionServer.getTimePelicula(_peliculas[0]);
                            control.SourceProvider.MediaPlayer.Time = time;
                        } catch (Exception e) {
                            control.SourceProvider.MediaPlayer.Time = 0;
                        }
                    }

                }
                control.SourceProvider.MediaPlayer.Audio.Volume = volumen;
                dp.Start();
                setTimeLabel();
                //resetMaximumTime();
                viewBoxPause.Visibility = Visibility.Visible;
                viewBoxPlay.Visibility = Visibility.Hidden;
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }


        public void nextVideo() {
            try {
                dp.Stop();
                long time = control.SourceProvider.MediaPlayer.Time;
                if (isOnline) {
                    if (_capitulos != null) {
                        ConexionServer.updateTiempoActualCap(_capitulos[currentVideoPosition], time);
                    } else if (_peliculas != null) {
                        ConexionServer.updateTiempoActualPel(_peliculas[currentVideoPosition], time);
                    }

                }
                cont = 0;
                if (lista != null) {
                    if (currentVideoPosition == lista.Length - 1) {

                        currentVideoPosition = 0;

                    } else {
                        currentVideoPosition++;
                    }
                    videoTitle.Content = names[currentVideoPosition];
                    setVideo(lista[currentVideoPosition]);
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

        }

        public void previousVideo() {
            dp.Stop();
            long time = control.SourceProvider.MediaPlayer.Time;
            if (isOnline) {
                if (_capitulos != null) {
                    ConexionServer.updateTiempoActualCap(_capitulos[currentVideoPosition], time);
                } else if (_peliculas != null) {
                    ConexionServer.updateTiempoActualPel(_peliculas[currentVideoPosition], time);
                }

            }
            cont = 0;
            if (currentVideoPosition == 0) {
                currentVideoPosition = lista.Length - 1;

            } else {
                currentVideoPosition--;
            }
            videoTitle.Content = names[currentVideoPosition];
            setVideo(lista[currentVideoPosition]);
        }

        private void advanceTimeClick(object sender, RoutedEventArgs e) {
            advanceTime();
        }

        private void backTimeClick(object sender, RoutedEventArgs e) {
            backTime();
        }


        public void advanceTime() {
            long time = videoPlayerProperties.advanceTime;
            long actualTime = control.SourceProvider.MediaPlayer.Time;
            if (actualTime + time <= control.SourceProvider.MediaPlayer.Length) {
                control.SourceProvider.MediaPlayer.Time += time;
                timeLine.Value = control.SourceProvider.MediaPlayer.Time;
            } else {
                control.SourceProvider.MediaPlayer.Time = control.SourceProvider.MediaPlayer.Length;
                timeLine.Value = control.SourceProvider.MediaPlayer.Time;
            }
            setTimeLabel();
        }


        public void backTime() {
            long time = videoPlayerProperties.backTime;
            long actualTime = control.SourceProvider.MediaPlayer.Time;
            if (actualTime - time >= 0) {
                control.SourceProvider.MediaPlayer.Time -= time;
                timeLine.Value = control.SourceProvider.MediaPlayer.Time;
            } else {
                control.SourceProvider.MediaPlayer.Time = 0;
                timeLine.Value = control.SourceProvider.MediaPlayer.Time;
            }
            setTimeLabel();
        }


        public long getTime() {
            return control.SourceProvider.MediaPlayer.Time;
        }


        private void resetControl() {
            control.SourceProvider.Dispose();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            control.SourceProvider.CreatePlayer(libDirectory);
        }

        private void timeLine_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            dp.Stop();
            isClicking = true;
        }


        private void timeLine_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            isClicking = false;
            setTimeLabel();
            control.SourceProvider.MediaPlayer.Play();
            dp.Start();
        }


        private void timeLine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            isClicking = false;
            dp.Start();
            setTimeLabel();
            control.SourceProvider.MediaPlayer.Time = (long)timeLine.Value;
        }

        private void timeLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            dp.Stop();
            isClicking = true;
            control.SourceProvider.MediaPlayer.Time = (long)timeLine.Value;

            Console.WriteLine(timeLine.Value);
            setTimeLabel();
        }


        private void TrackTime_ValueChanged(object sender, EventArgs e) {
            if (isClicking) {
                if (control.SourceProvider.MediaPlayer.IsPlaying()) {
                    control.SourceProvider.MediaPlayer.Time = (long)timeLine.Value;
                } else {
                    control.SourceProvider.MediaPlayer.Time = (long)timeLine.Value;
                }
                
                
            }
        }


        private void bExpandClick(object sender, RoutedEventArgs e) {
            videoPlayerProperties.setFullScreen();
        }


        private void bSearchVideoClick(object sender, EventArgs e) {

        }

        private void bPlayClick(object sender, EventArgs e) {
            playPause();
        }

        private void bNextVideoClick(object sender, EventArgs e) {
            nextVideo();
        }

        private void TrackVolume_ValueChanged(object sender, EventArgs e) {
            Slider slideVolume = (Slider)sender;
            setVolume((int)slideVolume.Value);
        }


        private void gridControles_MouseLeave(object sender, MouseEventArgs e) {
            gridControles.Background = new SolidColorBrush(Color.FromArgb(0, 23, 23, 23));

            gridBotonesPrincipales.Visibility = Visibility.Hidden;
            dockTimeLine.Visibility = Visibility.Hidden;
            gridVolume.Visibility = Visibility.Hidden;
            gridOthers.Visibility = Visibility.Hidden;

        }


        private void bPreviousVideoClick(object sender, EventArgs e) {
            previousVideo();
        }

        private void UserControl_PreviewKeyUp(object sender, KeyEventArgs e) {
            videoPlayerProperties.findShortCutMethod(e.Key);

        }

        public void btnIncTime_Click(object sender, EventArgs e) {

        }

        public RowDefinition getButtonSpace() {
            return buttonSpace;
        }

        public Grid getGridControles() {
            return gridControles;
        }

        public GradientStop getMidColorStop() {
            return midColor;
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e) {
            //if (e.Key == Key.Left) {
            //    backTime();
            //} else if (e.Key == Key.Right) {
            //    advanceTime();

            //}
            //videoPlayerProperties.findShortCutMethod(e.Key);
            videoPlayerProperties.findShortCutMethod(e.Key);

        }

        

        private void gridControles_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            gridControles.Focus();
        }

        private void gridControles_MouseEnter(object sender, MouseEventArgs e) {
            gridControles.Background = new SolidColorBrush(Color.FromArgb(255, 23, 23, 23));

            gridBotonesPrincipales.Visibility = Visibility.Visible;
            dockTimeLine.Visibility = Visibility.Visible;
            gridVolume.Visibility = Visibility.Visible;
            gridOthers.Visibility = Visibility.Visible;
        }

        public GradientStop getLastColorStop() {
            return lastColor;
        }

        public RowDefinition getRowClose() {
            return closeGrid;
        }

        public void setListaNombres(string[] titles) {
            names = titles;
        }

        public void CerrarReproductor(object sender, EventArgs e) {
            try {
                dp.Stop();
                long time = control.SourceProvider.MediaPlayer.Time;
                if (isOnline) {
                    if (_capitulos != null) {
                        ConexionServer.updateTiempoActualCap(_capitulos[currentVideoPosition], time);
                    } else if (_peliculas != null) {
                        ConexionServer.updateTiempoActualPel(_peliculas[0], time);
                    }
                }

            } catch (Exception ex) {

            }
            control.Dispose();
            main.hideControl();
        }
    }
}
