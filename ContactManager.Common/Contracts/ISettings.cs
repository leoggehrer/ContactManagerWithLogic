//@CodeCopy
namespace ContactManager.Common.Contracts
{
    public interface ISettings
    {
        string? this[string key] { get; }
    }
}