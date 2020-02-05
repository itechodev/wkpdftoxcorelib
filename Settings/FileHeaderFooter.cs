namespace wkpdftoxcorelib.Settings
{
    public class FileHeaderFooter : HeaderFooter
    {
        public FileHeaderFooter(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
    
}
