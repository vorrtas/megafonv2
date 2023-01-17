using System.Globalization;

using Megafon.Contracts.Interfaces;

namespace Megafon.Domain.Services;

public class CultureSerivce : ICultureService
{
    public void SetDefaultCulture()
    {
        CultureInfo? culture = CultureInfo.InvariantCulture.Clone() as CultureInfo;

        if (culture == null)
        {
            throw new ArgumentNullException(nameof(culture));
        }

        culture.DateTimeFormat = new DateTimeFormatInfo()
        {
            FullDateTimePattern = "yyyy-MM-dd HH:mm:ss",
            LongDatePattern = "yyyy-MM-dd",
            ShortDatePattern = "yyyy-MM-dd"
        };

        culture.NumberFormat = new NumberFormatInfo()
        {
            NumberGroupSeparator = " ",
            NumberDecimalDigits = 2,
        };

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
