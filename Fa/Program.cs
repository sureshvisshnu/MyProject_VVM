using fa.views;
using Fa.views.utils;
using FADataAccessLibrary.Configuration;
using Sentry.Protocol;

namespace Fa
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            ////SplashScreen SplashScreen = new SplashScreen();
            //BerklySoftStartUp BerklySoftStartUp = new BerklySoftStartUp();
            //Application.Run(BerklySoftStartUp);

            ApplicationConfiguration.Initialize();
            SplashScreen SplashScreen = new SplashScreen();
            Application.Run(SplashScreen);

            // If the user clicks "Enter," proceed to initialize and run SplashScreen
            // Application.Run(new SplashScreen());
        }
    }
}