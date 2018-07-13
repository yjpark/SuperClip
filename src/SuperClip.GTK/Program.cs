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

            var app = new SuperClip.App.Types.App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("Super Clip");
            window.Show();

            Gtk.Application.Run();
        }
    }
}