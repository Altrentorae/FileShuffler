using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DEBUG
#warning DEBUG BUILD
#endif
#if RELEASE
#warning RELEASE BUILD
#endif
namespace FileShuffler {
    class Program {

        static bool outputToWindow = true, timecheck = true;
        static SearchOption searchOption = SearchOption.TopDirectoryOnly;

        static void Main(string[] args) {
            List<string> argList = new List<string>(args);
            //Help output
            if (args.Contains("--help")) {
                Console.WriteLine(@"
    -ALL            : All filetypes excluding .exe
    -I              : If -ALL is present, include .exe
    -silent         : Hide per file output
    -silent--notime : Hide per file output AND per filetype time outputs
    -recurse        : Include all files in subdirectories, writes to active exe directory
    <extension>     : Include filetype (\>FileShuffler .jpg .csv .png)
    -<extension>    : Exclude filetype when -ALL is used (\>FileShuffler -ALL -.html)");
                return;
            }

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
            if (args.Contains("-ALL")) {
                //-ALL shuffles every file in directory
                bool inclusive = false; //Run with -ALL -I to include exes
                List<string> excludeList = new List<string>(); //Can exclude filetypes from -ALL with -<extension>
                foreach(string arg in args) {
                    if (arg[0] == '-' && arg[1] == '.') {
                        string nA = arg.Remove(0, 1);
                        excludeList.Add(nA);
                        argList.Remove(arg);
                    }
                }
                try {
                    if (args.Contains("-I")) { inclusive = true; }
                }
                catch (Exception) { }
                Console.WriteLine("Are you sure? Y/N");
                if (Console.ReadKey().Key == ConsoleKey.Y) {
                    string[] fileExts = getAllFilesExts();
                    foreach (string f in fileExts) {
                        if (excludeList.Contains(f)) {
                            continue;
                        }
                        if (f == ".exe" && inclusive) {
                            Shuffle(".exe");
                        }
                        else if (f != ".exe") {
                            Shuffle(f);
                        }
                    }
                }
            }
            
            argList.Remove("-silent");
            argList.Remove("-silent--notime");
            argList.Remove("-recurse");
            args = argList.Select(i => i.ToString()).ToArray();
            if (args.Length > 0) {    
                for (int i = 0; i < args.Length; i++) {
                    switch (args[i]) {
                        default: Shuffle(args[i]); break;
                        
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
