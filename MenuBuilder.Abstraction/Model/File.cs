namespace MenuBuilder.Abstraction.Model;

public class File
{
    public MenuFileInfo FileInfo { get; }

    public File(MenuFileInfo info)
    {
        FileInfo = info;
    }

    public virtual string ReadFile()
    {
        return string.Empty;
    }

    public virtual void WriteFile(string data)
    {

    } 
}
