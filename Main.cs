using System.Linq.Expressions;

namespace SMembersInfo
{
    public sealed class Main : RocketPlugin<Config>
    {
        public static Main Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;
        }

        #region Translations
        public override TranslationList DefaultTranslations => DefaultTranslationList;

        public new string Translate(string key, params object[] args) => base.Translate(key.Trim(TranslationKeyTrimCharacters), args);
        #endregion
    }
}