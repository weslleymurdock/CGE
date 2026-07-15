using cge.github.io.Application.Abstract.Localization;
using Microsoft.Extensions.Localization;

namespace cge.github.io.Application.Concrete.Localization;

public class Localizer(IStringLocalizerFactory factory) : ILocalizer
{
    private readonly IStringLocalizer _localizer = factory.Create(
            "Localization",
            typeof(Localizer).Assembly.FullName!
        );

    public string this[string key] => _localizer[key];
}