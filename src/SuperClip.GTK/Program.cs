using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace SuperClip.GTK
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();

            var app = SuperClip.App.App.initApplication();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("Super Clip");
            window.Show();

            Gtk.Application.Run();
        }
    }
}