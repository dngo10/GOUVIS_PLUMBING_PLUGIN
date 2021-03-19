using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.ProjectStructure
{
    class FileStructure
    {
        public string projectNumber;
        public string sqlFileName;
        public HashSet<FileData> nodePath;
        public HashSet<FileData> shbdPath;
        public HashSet<FileData> background;
        public HashSet<FileData> others;

        public FileStructure()
        {
            
        }
        
    }

    class FileData
    {
        public string path;
        public string fullPath;
        public string modified_date;
    }
}
