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
    public class KeyList {
        private ICollection<PlayerProperty> listaKeyEvents;

        public KeyList() {
            listaKeyEvents = new List<PlayerProperty>();
        }

        public KeyList(ICollection<PlayerProperty> keyEventList) {
            listaKeyEvents = keyEventList;
        }

        public void addPropertyShortcut(PlayerProperty p) {
            listaKeyEvents.Add(p);
        }

        public PlayerProperty findShortCutMethod(Key keyValue) {
            PlayerProperty finded = findKeyEvent(keyValue);
            if (finded != null) {
                return finded;
            }
            return null;

        }

        public PlayerProperty findKeyEvent(Key keyValue) {
            foreach (PlayerProperty k in listaKeyEvents) {
                if (k.value == keyValue) {
                    return k;
                }
            }
            return null;
        }
    }
}
