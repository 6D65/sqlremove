using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlremove
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = args[0];
            //string tableName = args[1];
            string fileName = "file.sql";
            string tableName = "PartialUpdateQueue";

            List<string> resultLines = new List<string>();

            //Console.WriteLine("Filename : " + fileName);
            //Console.WriteLine("TableName : " + tableName);

            bool foundStart = false;
            bool foundEnd = false;
            using (StreamReader reader = new StreamReader(fileName))
            {
                using (StreamWriter writer = new StreamWriter(string.Format("processed_removed_{0}_{1}", tableName, fileName)))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!foundStart)
                        { 
                            foundStart = isStartLine(line, tableName);
                        }
                        if (foundStart)
                        {
                            foundEnd = isEndLine(line, tableName);
                            //foundStart = !foundEnd;
                            if (foundEnd) foundStart = false;
                        }

                        if (foundStart || foundEnd)
                        {
                            resultLines.Add(line);
                        }
                        else
                        {
                            //writer.WriteLine(line);
                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(string.Format("result_{0}.sql", tableName)))
            {
                foreach (var line in resultLines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        static bool isStartLine(string line, string tableName)
        {
            if (line.Contains(tableName) && line.Contains("Adding") && line.Contains("START"))
            {
                return true;
            }
            return false;
        }

        static bool isEndLine(string line, string tableName)
        {
            if (line.Contains(tableName) && line.Contains("Adding") && line.Contains("END"))
            {
               return true;
            }
            return false;
        }
    }
}
