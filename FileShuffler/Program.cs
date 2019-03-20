using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileShuffler {
    class Program {
        static void Main(string[] args) {

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

        static void Shuffle(string extension) {

            string[] files = Directory.GetFiles(Environment.CurrentDirectory, $@"*{extension}");

            Random rnd = new Random();
            List<string> temp = new List<string>();
            for (int i = 0; i < files.Length; i++) {
                string s;
                while (true) {
                    s = rnd.Next(1, 10000).ToString();
                    if (temp.Contains(s)) {
                        continue;
                    }
                    else {
                        temp.Add(s);
                        break;
                    }
                }
            }
            string[] fnames = temp.ToArray();
            int fLen = fnames.Length;
            foreach (string file in files) {
                fnames = fnames.Where(x => x != file).ToArray(); //remove existing, probably from previous shuffle
            }

            foreach (string file in files) {

                string newName = fnames[rnd.Next(fnames.Length - 1)]; //get new name
                fnames = fnames.Where(x => x != newName).ToArray(); //remove new name from possible name
                File.Move(file, newName + $@"{extension}"); //rename file
                fnames = fnames.Where(x => x != newName).ToArray(); //remove new name from files, idk why I need to do it twice but it works
            }
        } 
    }
}
