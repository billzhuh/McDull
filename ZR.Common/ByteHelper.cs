using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace XUnit.Core.Helper
{
    public class ByteHelper
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// 将 byte[] 转成 Stream
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
