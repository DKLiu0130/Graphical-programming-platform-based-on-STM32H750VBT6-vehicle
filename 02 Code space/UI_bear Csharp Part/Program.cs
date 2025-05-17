using WinFormsApp1;
namespace UI_new

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
            ApplicationConfiguration.Initialize();
            //Application.Run(new RadMainForm());
            RadMainForm mainForm = new RadMainForm();
            myCtrLib.mainForm=mainForm;

            Application.Run(mainForm);
        }
    }
}