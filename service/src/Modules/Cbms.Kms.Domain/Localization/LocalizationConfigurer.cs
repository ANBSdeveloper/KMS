using Cbms.Localization;
using Cbms.Localization.Dictionaries;
using Cbms.Localization.Dictionaries.Xml;
using Cbms.Reflection.Extensions;

namespace Cbms.Kms.Domain.Localization
{
    public static class LocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(KmsConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(LocalizationConfigurer).GetAssembly(),
                        "Cbms.Kms.Domain.Localization.SourceFiles"
                    )
                )
            );

            localizationConfiguration.Languages.Add(new LanguageInfo("en", "English", null, false, false));
            localizationConfiguration.Languages.Add(new LanguageInfo("vi", "Vietnam", null, true, false));
        }
    }
}