using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Education.Languages
{
    public class DefaultLanguages : DefaultTypeInitializer<DefaultLanguages, Language>
    {
        public Language Battanian { get; private set; }
        public Language Vlandic { get; private set; }
        public Language Calradian { get; private set; }
        public Language Sturgian { get; private set; }
        public Language Aseran { get; private set; }
        public Language Khuzait { get; private set; }
        public Language Vakken { get; private set; }

        public override IEnumerable<Language> All
        {
            get
            {
                yield return Battanian;
                yield return Vlandic;
                yield return Calradian;
                yield return Sturgian;
                yield return Aseran;
                yield return Khuzait;
                yield return Vakken;
                foreach (Language item in ModAdditions)
                {
                    yield return item;
                }
            }
        }

        public override void Initialize()
        {
            var cultures = Game.Current.ObjectManager.GetObjectTypeList<CultureObject>();
            Battanian = new Language("language_battanian");
            Sturgian = new Language("language_sturgian");
            Khuzait = new Language("language_khuzait");
            Vlandic = new Language("language_vlandic");
            Calradian = new Language("language_calradian");
            Aseran = new Language("language_aseran");
            Vakken = new Language("language_vakken");

            Battanian.Initialize(new TextObject("{=tRp08jyH}Battanian"), new TextObject("{=!}"),
                cultures.First(x => x.StringId == "battania"), GetIntelligibles(Battanian));
            Sturgian.Initialize(new TextObject("{=VtNL32g2}Sturgian"), new TextObject("{=!}"),
                cultures.First(x => x.StringId == "sturgia"), GetIntelligibles(Sturgian));
            Khuzait.Initialize(new TextObject("{=ZdFBNgoJ}Khuzait"), new TextObject("{=!}"),
                cultures.First(x => x.StringId == "khuzait"), GetIntelligibles(Khuzait));
            Vlandic.Initialize(new TextObject("{=6FGQ31TM}Vlandic"), new TextObject("{=!}"),
                cultures.First(x => x.StringId == "vlandia"), GetIntelligibles(Vlandic));
            Calradian.Initialize(new TextObject("{=NWqkTdMt}Calradian"), 
                new TextObject("{=GmqBFSgN}The Imperial language of the Calradian empire. Though scholars have made efforts into keeping the language pure, centuries of contact with local cultures have made Calradian adopt small quantities of local vocabularies. Being a language of prestige, Calradian vocabulary are also often adopted by foreign languages, due to it's usefulness in the continent as a Lingua Franca, often used by traders, nobles during their education or peasants looking for a better life within the Empire."),
                cultures.First(x => x.StringId == "empire"), 
                GetIntelligibles(Calradian));
            Aseran.Initialize(new TextObject("{=UAeorLSO}Aseran"), new TextObject("{=!}"), cultures.First(x => x.StringId == "aserai"),
                GetIntelligibles(Aseran));
            Vakken.Initialize(new TextObject("{=brxz2SmN}Vakken"), new TextObject("{=!}"),
                cultures.First(x => x.StringId == "vakken"), GetIntelligibles(Vakken));
        }

        public Dictionary<Language, float> GetIntelligibles(Language language)
        {
            return language.StringId switch
            {
                "language_battanian" => new Dictionary<Language, float> {{Vakken, 0.15f}, {Calradian, 0.1f}},
                "language_vlandic" => new Dictionary<Language, float> {{Calradian, 0.15f}},
                "language_sturgian" => new Dictionary<Language, float> {{Vakken, 0.1f}},
                "language_calradian" => new Dictionary<Language, float> {{Vlandic, 0.1f}, {Battanian, 0.1f}},
                "language_vakken" => new Dictionary<Language, float> {{Battanian, 0.15f}, {Sturgian, 0.1f}},
                _ => new Dictionary<Language, float>()
            };
        }
    }
}