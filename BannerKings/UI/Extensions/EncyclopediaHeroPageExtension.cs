﻿using System.Collections.Generic;
using System.Xml;
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

namespace BannerKings.UI.Extensions
{
    [PrefabExtension("EncyclopediaHeroPage",
        "descendant::ListPanel[@Id='RightSideList']/Children/Widget[1]/Children/ListPanel[1]/Children",
        "EncyclopediaHeroPage")]
    internal class EncyclopediaHeroPageExtension : PrefabExtensionInsertPatch
    {
        private readonly List<XmlNode> nodes;

        public EncyclopediaHeroPageExtension()
        {
            var traits1 = new XmlDocument();
            traits1.LoadXml(
                "<EncyclopediaDivider Id=\"TraitsDivider\" MarginTop=\"20\" Parameter.Title=\"Traits\" Parameter.ItemList=\"..\\TraitsContainer\" GamepadNavigationIndex=\"0\"/>");
            var traits2 = new XmlDocument();
            traits2.LoadXml(
                "<NavigationScopeTargeter ScopeID=\"EncyclopediaHeroClanContentScope\" ScopeParent=\"..\\TraitsContainer\" ScopeMovements=\"Horizontal\" ExtendDiscoveryAreaTop=\"-10\"/>");
            var traits3 = new XmlDocument();
            traits3.LoadXml(
                "<Widget Id=\"TraitsContainer\" HeightSizePolicy=\"CoverChildren\" WidthSizePolicy=\"StretchToParent\"><Children><GridWidget DataSource=\"{TraitGroups}\" WidthSizePolicy=\"CoverChildren\" HeightSizePolicy=\"CoverChildren\" DefaultCellWidth=\"200\" DefaultCellHeight=\"200\" HorizontalAlignment=\"Center\" ColumnCount=\"4\" MarginTop=\"15\" MarginBottom=\"20\"><ItemTemplate><ListPanel WidthSizePolicy=\"StretchToParent\" HeightSizePolicy=\"CoverChildren\" StackLayout.LayoutMethod=\"VerticalBottomToTop\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Top\"><Children><AutoHideRichTextWidget HeightSizePolicy =\"CoverChildren\" WidthSizePolicy=\"StretchToParent\" VerticalAlignment=\"Center\" Brush=\"Encyclopedia.Stat.DefinitionText\" Text=\"@Title\" PositionYOffset=\"2\" /><GridWidget Id=\"StatsGrid\" DataSource=\"{Traits}\" WidthSizePolicy = \"StretchToParent\" HeightSizePolicy = \"CoverChildren\" DefaultCellWidth=\"275\" DefaultCellHeight=\"30\" HorizontalAlignment=\"Center\" ColumnCount=\"1\" MarginTop=\"10\" MarginLeft=\"15\"><ItemTemplate><Widget WidthSizePolicy=\"CoverChildren\" HeightSizePolicy=\"CoverChildren\" VerticalAlignment=\"Center\"><Children><ListPanel HeightSizePolicy =\"CoverChildren\" WidthSizePolicy=\"CoverChildren\" MarginLeft=\"15\" MarginTop=\"3\"><Children><AutoHideRichTextWidget HeightSizePolicy =\"CoverChildren\" WidthSizePolicy=\"CoverChildren\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Right\" Brush=\"Encyclopedia.Stat.DefinitionText\" Text=\"@Definition\" MarginRight=\"5\"/><AutoHideRichTextWidget HeightSizePolicy =\"CoverChildren\" WidthSizePolicy=\"CoverChildren\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Left\" Brush=\"Encyclopedia.Stat.ValueText\" Text=\"@Value\" PositionYOffset=\"2\" /></Children></ListPanel><HintWidget DataSource = \"{Hint}\" WidthSizePolicy=\"StretchToParent\" HeightSizePolicy=\"Fixed\" SuggestedHeight=\"15\" SuggestedWidth=\"100\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Center\" Command.HoverBegin=\"ExecuteBeginHint\" Command.HoverEnd=\"ExecuteEndHint\" /></Children></Widget></ItemTemplate></GridWidget></Children></ListPanel></ItemTemplate></GridWidget></Children></Widget>");

            nodes = new List<XmlNode>
                {traits1, traits2, traits3 };
        }

        public override InsertType Type => InsertType.Child;
        public override int Index => 4;

        [PrefabExtensionXmlNodes] public IEnumerable<XmlNode> Nodes => nodes;
    } 
}