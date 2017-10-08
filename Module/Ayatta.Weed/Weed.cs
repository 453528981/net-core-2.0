using System;

namespace Ayatta.Weed
{

    public class UploadResult
    {
        ///<summary>
        /// 用户Id
        ///</summary>
        public int Uid { get; set; }

        ///<summary>
        /// 目录Id
        ///</summary>
        public int Did { get; set; }

        /// <summary>
        /// Fid
        /// </summary>
        public string Fid { get; set; }       

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// FileUrl
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static implicit operator bool(UploadResult result)
        {
            return string.IsNullOrEmpty(result.Error);
        }
    }
}
