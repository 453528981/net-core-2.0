using System;
using System.Collections.Generic;
using System.Text;

namespace Ayatta.Web
{
    public class Weed
    {
        /// <summary>
        /// 当前所在目录
        /// </summary>
        public string Path { get; set; }

        public IList<File> Files { get; set; }

        public IList<Directory> Directories { get; set; }

        public string LastFileName { get; set; }

        public bool ShouldDisplayLoadMore { get; set; }

        public class Directory
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class File
        {
            public string Fid { get; set; }

            public string Name { get; set; }
        }
    }
}
