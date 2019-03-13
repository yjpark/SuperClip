using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Dap.Platform.Cli;
using Dap.UWP.Cli;
using IGuiApp = Dap.Gui.Types.IGuiApp;

namespace SuperClip.UWP {
    public class ThemeHook : IGuiAppHook {
        public void OnInit (IGuiApp app) {
            var frameworkElement = Window.Current.Content as FrameworkElement;
            if (frameworkElement != null) {
                if (app.Theme.Key == SuperClip.Fabulous.Theme.DarkTheme) {
                    frameworkElement.RequestedTheme = ElementTheme.Dark;
                } else {
                    frameworkElement.RequestedTheme = ElementTheme.Light;
                }
            }
        }
    }
}

