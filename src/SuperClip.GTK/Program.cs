using System;
using Xamarin.Forms.Platform.GTK;

namespace SuperClip.GTK
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Xamarin.Forms.Forms.Init();

            var window = new FormsWindow();
            window.LoadApplication(SuperClip.Forms.Helper.createApplication ());
            window.SetApplicationTitle("Super Clip");
            window.Show();

            Gtk.Application.Run();
        }
    }
}
