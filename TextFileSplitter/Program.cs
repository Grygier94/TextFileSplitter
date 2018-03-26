using System.IO;
using System.Configuration;

namespace TextFileSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputDirectory = ConfigurationManager.AppSettings["OutputDirectoryPath"];
            string outputFilePath = ConfigurationManager.AppSettings["OutputDirectoryPath"] + "/" + ConfigurationManager.AppSettings["OutputFile"];
            string outputExtension = ConfigurationManager.AppSettings["OutputExtension"];
            int linesLimit = int.Parse(ConfigurationManager.AppSettings["LinesLimit"]);
            string[] inputFiles = ConfigurationManager.AppSettings["InputFiles"].Split(',');

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            var iterator = int.Parse(ConfigurationManager.AppSettings["Iterator"]); ;
            foreach (var file in inputFiles)
            {
                StreamWriter writer = null;
                try
                {
                    using (StreamReader inputfile = new StreamReader(file))
                    {                        
                        int count = 0;
                        string line;
                        while ((line = inputfile.ReadLine()) != null)
                        {

                            if (writer == null || count >= linesLimit)
                            {
                                if (writer != null)
                                {
                                    writer.Close();
                                    writer = null;
                                }

                                writer = new StreamWriter(outputFilePath + iterator + outputExtension, true);
                                iterator++;
                                count = 0;
                            }

                            if(count == linesLimit-1 || inputfile.Peek() == -1)
                                writer.Write(line);
                            else
                                writer.WriteLine(line);

                            count++;
                        }
                    }
                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }
        }
    }
}