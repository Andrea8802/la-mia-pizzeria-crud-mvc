namespace la_mia_pizzeria_static
{
    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.AppendAllText("log.txt", "\nLOG: " + message);
        }
    }
}
