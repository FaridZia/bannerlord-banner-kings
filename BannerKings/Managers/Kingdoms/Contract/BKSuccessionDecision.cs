using System.Collections.Generic;
using System.Linq;
using BannerKings.Managers.Titles;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace BannerKings.Managers.Kingdoms.Contract
{
    public class BKSuccessionDecision : BKContractDecision
    {
        public BKSuccessionDecision(Clan proposerClan, SuccessionType governmentType, FeudalTitle title) : base(
            proposerClan, title)
        {
            successionType = governmentType;
        }

        [SaveableProperty(100)] private SuccessionType successionType { get; set; }

        public override void UpdateDecision(int value)
        {
            successionType = (SuccessionType)value;
        }
        public override void ApplyChosenOutcome(DecisionOutcome chosenOutcome)
        {
            var newGovernment = (chosenOutcome as SuccessionDecisionOutcome).ShouldDecisionBeEnforced;
            if (newGovernment)
            {
                Title.ChangeContract(successionType);
            }
        }

        public override float CalculateKingdomSupport(Kingdom kingdom)
        {
            var support = 0f;
            float clans = 0;
            foreach (var clan in kingdom.Clans)
            {
                if (!clan.IsUnderMercenaryService)
                {
                    if (clan == Clan.PlayerClan)
                    {
                        support += 100f;
                    }
                    else
                    {
                        support += DetermineSupport(clan, new SuccessionDecisionOutcome(true));
                    }

                    clans++;
                }
            }

            return MBMath.ClampFloat(support / clans, 0f, 100f);
        }


        public override void ApplySecondaryEffects(MBReadOnlyList<DecisionOutcome> possibleOutcomes, DecisionOutcome chosenOutcome)
        {
        }

        public override TextObject GetSecondaryEffects()
        {
            return null;
        }

        public override Clan DetermineChooser()
        {
            return Kingdom.RulingClan;
        }

        public override IEnumerable<DecisionOutcome> DetermineInitialCandidates()
        {
            yield return new SuccessionDecisionOutcome(true);
            yield return new SuccessionDecisionOutcome(false);
        }

        public override void DetermineSponsors(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
        {
            foreach (var decisionOutcome in possibleOutcomes)
            {
                if (((SuccessionDecisionOutcome) decisionOutcome).ShouldDecisionBeEnforced)
                {
                    decisionOutcome.SetSponsor(ProposerClan);
                }

                else
                {
                    AssignDefaultSponsor(decisionOutcome);
                }
            }
        }

        public override float DetermineSupport(Clan clan, DecisionOutcome possibleOutcome)
        {
            var policyDecisionOutcome = possibleOutcome as SuccessionDecisionOutcome;
            float authoritarian = clan.Leader.GetTraitLevel(DefaultTraits.Authoritarian);
            float egalitarian = clan.Leader.GetTraitLevel(DefaultTraits.Authoritarian);
            float oligarchic = clan.Leader.GetTraitLevel(DefaultTraits.Authoritarian);
            var weights = GetWeights();

            var num = weights[0] * authoritarian;
            var num2 = weights[1] * oligarchic;
            var num3 = weights[2] * egalitarian;

            var num4 = num + num3 + num2;

            if (clan == Kingdom.RulingClan)
            {
                if (successionType == SuccessionType.Hereditary_Monarchy && policyDecisionOutcome.ShouldDecisionBeEnforced)
                {
                    num4 += 2f;
                }
                else
                {
                    num4 -= 2f;
                }
            }

            float num5;
            if (policyDecisionOutcome.ShouldDecisionBeEnforced)
            {
                num5 = 60f;
            }
            else
            {
                num5 = -100f;
            }

            return num4 * num5;
        }

        private float[] GetWeights()
        {
            return successionType switch
            {
                SuccessionType.Hereditary_Monarchy => new[] {1f, 3f, -2f},
                SuccessionType.Elective_Monarchy => new[] {-1f, 5f, 1f},
                SuccessionType.Imperial => new[] {5f, -2f, -1f},
                _ => new[] {-3f, 1f, 5f}
            };
        }

        public override TextObject GetChooseDescription()
        {
            var textObject = new TextObject("{=mXcErd03}As {?IS_FEMALE}queen{?}king{\\?} you must decide whether to enforce the policy of {POLICY_NAME}.");
            textObject.SetTextVariable("IS_FEMALE", DetermineChooser().Leader.IsFemale ? 1 : 0);
            textObject.SetTextVariable("POLICY_NAME", Utils.TextHelper.GetName(successionType));
            return textObject;
        }

        public override TextObject GetChooseTitle()
        {
            var textObject = new TextObject("{=iZ6VfHe6}Change government to {GOVERNMENT}");
            textObject.SetTextVariable("GOVERNMENT", Utils.TextHelper.GetName(successionType));
            return textObject;
        }

        public override TextObject GetChosenOutcomeText(DecisionOutcome chosenOutcome, SupportStatus supportStatus,
            bool isShortVersion = false)
        {
            TextObject textObject;
            var newGovernment = ((SuccessionDecisionOutcome) chosenOutcome).ShouldDecisionBeEnforced;
            if (newGovernment)
            {
                textObject = new TextObject("{=u0mnqvO2}The {KINGDOM}'s government is now {POLICY_DESCRIPTION}. {POLICY_SUPPORT}");
            }
            else
            {
                textObject = new TextObject("{=ikYX00pK}The {KINGDOM}'s government will continue to be {POLICY_DESCRIPTION}. {POLICY_SUPPORT}");
            }


            textObject.SetTextVariable("KINGDOM", Kingdom.InformalName);
            textObject.SetTextVariable("POLICY_DESCRIPTION",
                newGovernment ? Utils.TextHelper.GetName(successionType) : Utils.TextHelper.GetName(Title.Contract.Succession));
            if (isShortVersion || IsSingleClanDecision())
            {
                textObject.SetTextVariable("POLICY_SUPPORT", TextObject.Empty);
            }
            else
            {
                textObject.SetTextVariable("POLICY_SUPPORT", "{=74XdPqxc}This decision caused a split in the council.");
                switch (supportStatus)
                {
                    case SupportStatus.Majority:
                        textObject.SetTextVariable("POLICY_SUPPORT",
                            "{=VPH5EBOT}This decision had the support of the council.");
                        break;
                    case SupportStatus.Minority:
                        textObject.SetTextVariable("POLICY_SUPPORT",
                            "{=PnCak8E1}This decision was rejected by the support of the council.");
                        break;
                }
            }

            return textObject;
        }

        public override TextObject GetGeneralTitle()
        {
            return new TextObject(successionType.ToString());
        }

        public override int GetProposalInfluenceCost()
        {
            return 250;
        }

        public override DecisionOutcome GetQueriedDecisionOutcome(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
        {
            return possibleOutcomes.FirstOrDefault(t => ((SuccessionDecisionOutcome) t).ShouldDecisionBeEnforced);
        }

        public override TextObject GetSupportDescription()
        {
            var textObject = new TextObject("{=JjgGFcCt}{CLAN} proposes a change of government, currently {CURRENT} to {PROPOSED}. You can pick your stance regarding this decision.");

            textObject.SetTextVariable("CLAN", DetermineChooser().Leader.Name);
            textObject.SetTextVariable("CURRENT",
                Utils.Helpers.GetGovernmentString(Title.Contract.Government, Kingdom.Culture));
            textObject.SetTextVariable("PROPOSED", Utils.TextHelper.GetName(successionType));
            return textObject;
        }

        public override TextObject GetSupportTitle()
        {
            var textObject = new TextObject("{=Sd4K6UiX}Vote to change realm's succession to {GOVERNMENT}");
            textObject.SetTextVariable("GOVERNMENT", Utils.TextHelper.GetName(successionType));
            return textObject;
        }

        public override bool IsAllowed()
        {
            var kingdom = ProposerClan.Kingdom;
            if (kingdom == null || FactionManager.GetEnemyKingdoms(kingdom).Any())
            {
                return false;
            }

            return base.IsAllowed();
        }

        public class SuccessionDecisionOutcome : DecisionOutcome
        {
            public SuccessionDecisionOutcome(bool shouldBeEnforced)
            {
                ShouldDecisionBeEnforced = shouldBeEnforced;
            }

            [SaveableProperty(200)] public bool ShouldDecisionBeEnforced { get; set; }


            public override TextObject GetDecisionTitle()
            {
                var textObject = new TextObject("{=DJRboTyg}{?SUPPORT}Yes{?}No{\\?}");
                textObject.SetTextVariable("SUPPORT", ShouldDecisionBeEnforced ? 1 : 0);
                return textObject;
            }

            public override TextObject GetDecisionDescription()
            {
                if (ShouldDecisionBeEnforced)
                {
                    return new TextObject("{=WEJOxOj8}We support this proposal");
                }

                return new TextObject("{=zfUqKqW2}We oppose this proposal");
            }

            public override string GetDecisionLink()
            {
                return null;
            }

            public override ImageIdentifier GetDecisionImageIdentifier()
            {
                return null;
            }
        }
    }
}