Place into the directory/folder you which to interact with, and run!
By default it will shuffle .jpg and .png | Other file types can be manually specified by passing the extension including . in the command prompt

`Directory>FileShuffler .png .tiff .jpeg .jpg`

You can also run with the `-ALL` flag to affect all file types in the directory. This does NOT affect .exe by default as to prevent renaming potentially important files
to rename exes include `-I` immediately following `-ALL`

`Directory>FileShuffler -ALL -I`

`-silent` : Hides per file output from console

`-silent--notime` : Hides per file output AND time per extension output

`-recurse` : Gathers files from all subdirectories AND top level, writes new files to ACTIVE directory. NOT to the file's original directory
