using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileShuffler {
    class Program {

        static bool outputToWindow = true, timecheck = true;
        static SearchOption searchOption = SearchOption.TopDirectoryOnly;

        static void Main(string[] args) {
            if (args.Contains("-silent")) { //Override console window output
                outputToWindow = false;
            }
            if (args.Contains("-silent--notime")) { //Override console window output
                outputToWindow = false;
                timecheck = false;
            }
            if (args.Contains("-recurse")) { //All subdirectories included
                searchOption = SearchOption.AllDirectories;
            }

            List<string> argList = new List<string>(args);
            argList.Remove("-silent");
            argList.Remove("-silent--notime");
            argList.Remove("-recurse");
            args = argList.Select(i => i.ToString()).ToArray();
            if (args.Length > 0) {    
                for (int i = 0; i < args.Length; i++) {
                    switch (args[i]) {
                        default: Shuffle(args[i]); break;
                        case "-ALL": //-ALL shuffles every file in directory
                            bool inclusive = false; //Run with -ALL -I to include exes
                            try {
                                if (args[i + 1] == "-I") { inclusive = true; }
                            }
                            catch (Exception) { }
                            Console.WriteLine("Are you sure? Y/N");
                            if(Console.ReadKey().Key == ConsoleKey.Y) {
                                string[] fileExts = getAllFilesExts();
                                foreach(string f in fileExts) {

                                    if (f == ".exe" && inclusive) {
                                        Shuffle(".exe");
                                    }
                                    else if (f != ".exe"){
                                        Shuffle(f);
                                    }
                                }
                            }
                            break;
                    }
                    
                }
            }
            else {
                Shuffle(".jpg");
                Shuffle(".png");
            }

        }
        static string[] getAllFilesExts() {
            string[] files = Directory.GetFiles(Environment.CurrentDirectory);
            List<string> fileExts = new List<string>();
            foreach(string file in files) {
                string e = Path.GetExtension(file);
                if (!fileExts.Contains(e)) {
                    fileExts.Add(e);
                }
            }
            return fileExts.ToArray();
        }


        static List<string> fileList;
        static int getNum(string[] files, int num, string exten) {
            
            System.Diagnostics.Debug.WriteLine(Environment.CurrentDirectory+"\\"+num.ToString()+exten);
            System.Diagnostics.Debug.WriteLine(files.ToList().Contains(Environment.CurrentDirectory + "\\" + num.ToString() + exten));
            if (files.ToList().Contains(Environment.CurrentDirectory + "\\" + num.ToString() + exten)) {
                num++;
                return getNum(files, num, exten);
            }
            if (num == 10) {
                num++;
                return getNum(files, num, exten);
            }
            fileList.Add(Environment.CurrentDirectory + "\\" + num.ToString() + exten);
            return num;
            
        }


        static void Shuffle(string extension) {
            DateTime time = DateTime.Now;
            if (timecheck) { Console.WriteLine($@"----------Shuffling {extension}----------"); }
            string[] files = Directory.GetFiles(Environment.CurrentDirectory, $@"*{extension}",searchOption);
            fileList = files.ToList();
            fileList.ShuffleList();
            string fileListSize = fileList.Count.ToString();
            for (int i=0; i < files.Length; i++) {
                System.Diagnostics.Debug.WriteLine($"----------{i}----------");
                string newName = getNum(fileList.ToArray(),i,extension).ToString();;
                if (outputToWindow) { Console.WriteLine($"({i.ToString().PadLeft(6, '0')} of {fileListSize} | {fileList[i]} => {newName}"); }
                fileList.Add(Environment.CurrentDirectory + "\\" + i.ToString() + extension);
                File.Move(fileList[i], newName + $@"{extension}"); //rename file
                
            }
            TimeSpan timeSpan = (DateTime.Now - time);
            if (timecheck) { Console.WriteLine($@"----------{extension} Finished in {timeSpan.Minutes} Mins : {timeSpan.Seconds} Secs----------"); }
        } 
    }
}
