using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoJsonFileMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter a directory : ");
                var dir = Console.ReadLine();

                if (!IsDirectoryEmpty(dir))
                {
                    DirectoryInfo d = new DirectoryInfo(dir);
                    FileInfo[] files = d.GetFiles("*.geojson"); //Getting Text files
                    files.FirstOrDefault(x => x.Name.Contains("_total"))?.Delete();
                    files = d.GetFiles("*.geojson");//update after delete

                    var fileName = files.Select(x => x.Name).FirstOrDefault();
                    if (fileName.Contains("_markers"))
                    {
                        fileName = fileName.Split(new string[] { "_markers" }, StringSplitOptions.None)[0] + "_total.geojson";
                    }
                    else
                    {
                        fileName = fileName.Split('.')[0] + "_total.geojson";
                    }

                    string mergedText = "";
                    foreach (var file in files)
                    {
                        string content = File.ReadAllText(file.FullName);
                        JObject json = JObject.Parse(content);
                        if (json["features"] != null)
                        {
                            mergedText += json["features"].ToString().Substring(1, json["features"].ToString().Length - 2);
                        }
                    }
                    File.WriteAllText(Path.Combine(d.FullName, fileName), "[" + mergedText + "]");
                }
                else
                {
                    Console.WriteLine("No files found under given directory.");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured. " + ex.Message);
            }
            Console.ReadLine();
        }
        private static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}
