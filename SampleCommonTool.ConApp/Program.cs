namespace SampleCommonTool.ConApp
{
    /// <summary>
    /// Represents the entry point of the application.
    /// </summary>
    public partial class Program
    {
        public static void Main(string[] args)
        {
            SampleApp app = new();

            app.Run(args);
        }
    }
}
