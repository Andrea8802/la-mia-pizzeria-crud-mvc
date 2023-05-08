namespace la_mia_pizzeria_static
{
    public class CustomConsoleLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            Console.WriteLine("\nLOG: " + message);
        }
    }
}
