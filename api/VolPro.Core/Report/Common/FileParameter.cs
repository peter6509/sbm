namespace VolPro.Core.common
{
    public class FileParameter
    {

        //report=1a&data=Customer&type=pdf&open=inline
        /// <summary>
        /// 報表模板
        /// </summary>
        public string report { get; set; }
        /// <summary>
        /// 報表數據
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 文件類型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 下载还是浏览器内联打開
        /// </summary>
        public string open { get; set; }
        /// <summary>
        /// 图片類型
        /// </summary>
        public string img { get; set; }
        /// <summary>
        /// 下载文件名
        /// </summary>
        public string filename { get; set; }

    }

}
