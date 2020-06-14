using ProyectoWPF.Reproductor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vlc.DotNet.Wpf;

namespace ProyectoWPF.Reproductor {
    public class VideoPlayerProperties {
        public long advanceTime { get; set; }
        public long backTime { get; set; }
        public long increaseSpeed { get; set; }
        public long decreaseSpeed { get; set; }
        //public long increaseZoom { get; set; }
        //public long decreaseZoom { get; set; }

        private bool isFullscreen = false;
        public VI_Reproductor parentUser { get; set; }
        public VlcControl control { get; set; }
        public KeyList shortCutList { get; set; }

        public delegate string TestDelegate();
        public VideoPlayerProperties(VI_Reproductor userParent, VlcControl control) {
            this.parentUser = userParent;
            this.control = control;
            advanceTime = 5000;
            backTime = 5000;
            shortCutList = new KeyList();
            PlayerProperty advTime = new PlayerProperty(Key.Right, "advanceTime", "Adelanta el tiempo X segundos");
            PlayerProperty bkTime = new PlayerProperty(Key.Left, "backTime", "Atrasa el tiempo X segundos");
            shortCutList.addPropertyShortcut(advTime);
            shortCutList.addPropertyShortcut(bkTime);
        }

        public void setFullScreen() {
            if (isFullscreen) {
                isFullscreen = false;
                parentUser.getRowClose().Height = new GridLength(30,GridUnitType.Pixel);
            } else {
                isFullscreen = true;
                parentUser.getRowClose().Height = new GridLength(0, GridUnitType.Pixel);
            }
        }

        public void findShortCutMethod(Key k) {
            PlayerProperty p = shortCutList.findShortCutMethod(k);
            if (p != null) {
                switch (p.name) {
                    case "advanceTime":
                        parentUser.advanceTime();
                        break;
                    case "backTime":
                        parentUser.backTime();
                        break;
                }
            }

        }

        public bool isFullScreen() {
            return isFullscreen;
        }


    }
}
