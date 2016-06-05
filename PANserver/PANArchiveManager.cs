using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PANserver
{
    class PANArchiveManager
    {
        public void CreateLists(string fileName, out List<string> panList, out List<string> maskList)
        {
            var reader = new StreamReader(fileName);
            panList = new List<string>();
            maskList = new List<string>();
            panList.Clear();
            maskList.Clear();

            while ((!reader.EndOfStream))
            {
                string currentRow = reader.ReadLine();
                panList.Add(currentRow.Substring(0, 16));
                maskList.Add(currentRow.Substring(17, 16));
            }
            reader.Close();
        }

        public void SaveLists(string fileName, List<string> panList, List<string> maskList)
        {
            var writer = new StreamWriter(fileName);

            for (int i = 0; i < panList.Count(); i++)
            {
                string fileLine = panList[i] + " " + maskList[i];
                writer.WriteLine(fileLine);
            }
            writer.Close();
        }
    }
}
