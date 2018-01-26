namespace imow.Framework.Cache.DistributedCache.Compress
{
    /// <summary>
    /// 字符串压缩
    /// </summary>
    public interface ICompressProvider
    {
         string CompressString(string text);

         string DecompressString(string compressedText);
    }
}
