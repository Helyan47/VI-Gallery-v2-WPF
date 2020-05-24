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

namespace Reproductor {
    public class VideoPlayerProperties {
        public long advanceTime { get; set; }
        public long backTime { get; set; }
        public long increaseSpeed { get; set; }
        public long decreaseSpeed { get; set; }
        //public long increaseZoom { get; set; }
        //public long decreaseZoom { get; set; }

        private bool isFullscreen = false;
        public Window parentWindow { get; set; }
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
                parentUser.getButtonSpace().Height = new GridLength(120);
                parentUser.getGridControles().SetValue(Grid.RowProperty, 1);
                isFullscreen = false;

                /*Color aux = Color.FromArgb(255, 23, 23, 23);
                parentUser.getMidColorStop().Color = aux;
                aux = Color.FromArgb(255, 23, 23, 23);
                parentUser.getLastColorStop().Color = aux;*/
            } else {
                parentUser.getButtonSpace().Height = new GridLength(0);
                parentUser.getGridControles().SetValue(Grid.RowProperty, 0);
                isFullscreen = true;
                /*Color aux = Color.FromArgb(74, 23, 23, 23);
                parentUser.getMidColorStop().Color = aux;
                aux = Color.FromArgb(0, 23, 23, 23);
                parentUser.getLastColorStop().Color = aux;*/
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
