namespace cge.github.io.Application.Abstract.Localization;

public interface ILocalizer
{
    string this[string key] { get; }
}
