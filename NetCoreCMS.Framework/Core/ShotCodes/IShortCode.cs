namespace NetCoreCMS.Framework.Core.ShotCodes
{
    /// <summary>
    /// Interface for implementing ShortCode.
    /// You must have set ShortCodeName. Which will be unique name.
    /// ShortCode of the content will be replaced by the returned string of Render() method.
    /// </summary>
    public interface IShortCode
    {
        string ShortCodeName { get; }
        string Render(params object[] paramiters);
    }
}
