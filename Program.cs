using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace File_Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
            Renamer();
        }

        public static void Renamer()
        {

            String Line;
            List<String> Names = new List<string>();
            System.Console.WriteLine("Enter list of names then ctrl+z and then enter.");
            while ((Line = Console.ReadLine()) != null) {
                Names.Add(Line);
            }
            System.Console.WriteLine(@"Are the names in ""Last, First"" (Y/N)");
            // String Order = Console.ReadLine();
            if(Console.ReadLine().ToUpper() == "Y")
            {
                Names = Flipper(Names);
            }

            // Get the directory and the files to rename
            System.Console.WriteLine("Enter folder path.");
            String FilePath = System.Console.ReadLine();
            DirectoryInfo dir = new DirectoryInfo(@FilePath);

            // Get the files in the directory
            FileInfo[] Files = dir.GetFiles();
            foreach(FileInfo CurrentFile in Files)
            {
                String FileName = Regex.Replace(Path.GetFileNameWithoutExtension(CurrentFile.FullName), @"[_0-9]+", "");
                String Ex =  Path.GetExtension(CurrentFile.FullName);

                foreach(String Name in Names)
                {
                    if(Name.ToLower().StartsWith(FileName.ToLower()))
                    {
                        int Count = 1;
                        String NewFileName = CurrentFile.FullName.Replace(CurrentFile.Name, $"{Name}{Ex}");

                        // Checks to see if a file with the same name exists.
                        while(File.Exists(NewFileName))
                        {
                            String TempFileName = $"{Name} ({Count++}){Ex}";
                            NewFileName = CurrentFile.FullName.Replace(CurrentFile.Name, TempFileName);
                        }
                        File.Move(CurrentFile.FullName, NewFileName);
                        break;
                    }
                }
            }
        }

        public static List<String> Flipper(List<String> Names)
        {
            List<String> ReversedNames = new List<String>();
            foreach(String Name in Names)
            {
                String NameOne = "";
                String NameTwo = "";
                Boolean Switch = false;
                for(int i = 0; i < Name.Length; i++)
                {
                    if(Name[i].ToString() == ",")
                    {
                        Switch = true;
                        i++;
                    }
                    if(!Switch)
                    {
                        NameOne += Name[i];
                    }
                    else
                    {
                        NameTwo += Name[i];
                    }
                }
                String Reversed = $"{NameTwo}, {NameOne}".Trim();
                ReversedNames.Add(Reversed);
            }
            return ReversedNames;
        }
    }
}
