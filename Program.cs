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

            // Takes a list of names until the escape character is reached.
            System.Console.WriteLine("Enter list of names then ctrl+z and then enter.");
            while ((Line = Console.ReadLine()) != null) {
                Names.Add(Line);
            }

            // Gets the format of the names in the list provided.
            System.Console.WriteLine(@"Are the names in the list ""Last, First""? (Y/N)");
            String NameFormat = Console.ReadLine().ToUpper()[0].ToString();

            // Gets the format of the file names.
            System.Console.WriteLine("Do the files contain part of the first name or last name? (F/L)?");
            String FileFormat = Console.ReadLine().ToUpper()[0].ToString();

            // Flips the list of names to match the file format.
            Boolean Flipped = false;
            if((NameFormat == "Y" && FileFormat == "F") || (NameFormat == "N" && FileFormat == "L"))
            {
                Names = Flipper(Names);
                Flipped = true;
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
                    // Retains the list given by user.
                    String TempName = Name;
                    if(Flipped)
                    {
                        TempName = Flipper(Name);
                    }

                    // Renames the file
                    if(Name.ToLower().StartsWith(FileName.ToLower()))
                    {
                        int Count = 1;
                        String NewFileName = CurrentFile.FullName.Replace(CurrentFile.Name, $"{TempName}{Ex}");

                        // Checks to see if a file with the same name exists.
                        while(File.Exists(NewFileName))
                        {
                            String TempFileName = $"{TempName} ({Count++}){Ex}";
                            NewFileName = CurrentFile.FullName.Replace(CurrentFile.Name, TempFileName);
                        }
                        File.Move(CurrentFile.FullName, NewFileName);
                        break;
                    }
                }
            }
        }

        // Switches the format of the names.
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
        
        // Switches the format of a single name
        public static String Flipper(String Name)
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
            return $"{NameTwo}, {NameOne}".Trim();
        }
    }
}
