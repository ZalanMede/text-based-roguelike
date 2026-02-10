using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class FileIO
    {
        // Újra kell írnom, mert a DLL felülírja a txt fájlt, nem hozzáír :c
        public static void WriteToFile(string fileName, List<string> listOfStrings, bool needToAppend)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, needToAppend);
                listOfStrings.ForEach(s => sw.WriteLine((s)));
                sw.Close();
                Console.WriteLine($"Run information saved in: {fileName}");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
