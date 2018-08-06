﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.2.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace AgateLib.Tests.UserInterface.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class FF6MenuFeature : Xunit.IClassFixture<FF6MenuFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "FF6Menu.feature"
#line hidden
        
        public FF6MenuFeature(FF6MenuFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FF6Menu", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 3
#line 4
 testRunner.Given("I have the FF6 system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "HP",
                        "MaxHP"});
            table1.AddRow(new string[] {
                        "Tora",
                        "70",
                        "100"});
            table1.AddRow(new string[] {
                        "Unlocke",
                        "100",
                        "100"});
            table1.AddRow(new string[] {
                        "Deadgar",
                        "100",
                        "100"});
            table1.AddRow(new string[] {
                        "Sabo",
                        "100",
                        "100"});
#line 5
 testRunner.Given("a party of", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ItemType",
                        "Effect"});
            table2.AddRow(new string[] {
                        "Short Sword",
                        "Weapon",
                        ""});
            table2.AddRow(new string[] {
                        "Long Sword",
                        "Weapon",
                        ""});
            table2.AddRow(new string[] {
                        "Leather Shield",
                        "Shield",
                        ""});
            table2.AddRow(new string[] {
                        "Leather Armor",
                        "Armor",
                        ""});
            table2.AddRow(new string[] {
                        "Leather Helm",
                        "Helm",
                        ""});
            table2.AddRow(new string[] {
                        "Sprint Shoes",
                        "Relic",
                        ""});
            table2.AddRow(new string[] {
                        "Running Shoes",
                        "Relic",
                        ""});
            table2.AddRow(new string[] {
                        "White Cape",
                        "Relic",
                        ""});
            table2.AddRow(new string[] {
                        "Dried Meat",
                        "Item",
                        "heal 999"});
#line 11
 testRunner.And("an inventory of", ((string)(null)), table2, "And ");
#line 22
 testRunner.And("I open the menu", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Open Items Menu")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Open Items Menu")]
        public virtual void FF6_OpenItemsMenu()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Open Items Menu", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 25
 testRunner.When("I select Items", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
 testRunner.Then("the active window is items/items", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Use Item")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Use Item")]
        public virtual void FF6_UseItem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Use Item", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 29
 testRunner.When("I select Items, Dried Meat, Dried Meat, Tora", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 30
 testRunner.Then("Tora is healed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Reorder items manually")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Reorder items manually")]
        public virtual void FF6_ReorderItemsManually()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Reorder items manually", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 33
 testRunner.When("I select Items", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 34
 testRunner.And("I press the down button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.And("I press the accept button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.Then("Long Sword is the active menu item", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
 testRunner.When("I press the down button 2 times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.And("I press the accept button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.Then("Leather Armor is in slot 1 in the inventory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.And("Long Sword is in slot 3 in the inventory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Open Skills Menu")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Open Skills Menu")]
        public virtual void FF6_OpenSkillsMenu()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Open Skills Menu", ((string[])(null)));
#line 42
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 43
 testRunner.When("I select Skills, Tora", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
 testRunner.Then("the active window is skills/skillType", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Open Items Menu and Return to Main Menu")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Open Items Menu and Return to Main Menu")]
        public virtual void FF6_OpenItemsMenuAndReturnToMainMenu()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Open Items Menu and Return to Main Menu", ((string[])(null)));
#line 50
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 51
 testRunner.When("I select Items", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
 testRunner.And("I press the cancel button 2 times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
 testRunner.Then("the default workspace is active", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 54
 testRunner.And("the active window is default/main", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Arrange items")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Arrange items")]
        public virtual void FF6_ArrangeItems()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Arrange items", ((string[])(null)));
#line 62
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 63
 testRunner.When("I select Items", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 64
 testRunner.And("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.And("I select Arrange", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
 testRunner.Then("the items are arranged", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Activate magic")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Activate magic")]
        public virtual void FF6_ActivateMagic()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Activate magic", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 69
 testRunner.When("I select Skills, Tora, Magic", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 70
 testRunner.Then("the magic workspace is active", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 71
 testRunner.When("I press the cancel button 2 times", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 72
 testRunner.Then("the active window is default/main", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Activate Espers")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Activate Espers")]
        public virtual void FF6_ActivateEspers()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Activate Espers", ((string[])(null)));
#line 74
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 75
 testRunner.When("I select Skills, Tora, Espers", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 76
 testRunner.Then("the Espers workspace is active", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="FF6 - Equip Items")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Equip Items")]
        [Xunit.InlineDataAttribute("R-hand", "Short Sword", new string[0])]
        [Xunit.InlineDataAttribute("L-hand", "Leather Shield", new string[0])]
        [Xunit.InlineDataAttribute("Head", "Leather Helm", new string[0])]
        [Xunit.InlineDataAttribute("Body", "Leather Armor", new string[0])]
        public virtual void FF6_EquipItems(string slot, string item, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Equip Items", exampleTags);
#line 78
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 79
 testRunner.When(string.Format("I select Equip, Tora, Equip, {0}, {1}", slot, item), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 80
 testRunner.Then(string.Format("{0} is equipped on Tora in the {1} slot", item, slot), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Cancel PC selection after equip")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Cancel PC selection after equip")]
        public virtual void FF6_CancelPCSelectionAfterEquip()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Cancel PC selection after equip", ((string[])(null)));
#line 89
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 90
 testRunner.When("I select Equip", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 91
 testRunner.And("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
 testRunner.Then("the active window is default/main", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Exit equip menus")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Exit equip menus")]
        public virtual void FF6_ExitEquipMenus()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Exit equip menus", ((string[])(null)));
#line 94
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 95
 testRunner.When("I select Equip, Tora, Equip, R-hand", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 96
 testRunner.And("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
 testRunner.Then("the active window is equip/slots", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 98
 testRunner.When("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 99
 testRunner.Then("the active window is equip/equipActionType", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 100
 testRunner.When("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 101
 testRunner.Then("the active window is default/main", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Equip multiple items in succession")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Equip multiple items in succession")]
        public virtual void FF6_EquipMultipleItemsInSuccession()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Equip multiple items in succession", ((string[])(null)));
#line 103
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 104
 testRunner.When("I select Equip, Tora, Equip, R-hand, Short Sword, L-hand, Leather Shield, Head, L" +
                    "eather Helm, Body, Leather Armor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 105
 testRunner.Then("Short Sword is equipped on Tora in the R-hand slot", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 106
 testRunner.Then("Leather Shield is equipped on Tora in the L-hand slot", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 107
 testRunner.Then("Leather Helm is equipped on Tora in the Head slot", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 108
 testRunner.Then("Leather Armor is equipped on Tora in the Body slot", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="FF6 - Optimum equipment for completely unequipped character.")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Optimum equipment for completely unequipped character.")]
        [Xunit.InlineDataAttribute("R-hand", "Short Sword", new string[0])]
        [Xunit.InlineDataAttribute("L-hand", "Leather Shield", new string[0])]
        [Xunit.InlineDataAttribute("Head", "Leather Helm", new string[0])]
        [Xunit.InlineDataAttribute("Body", "Leather Armor", new string[0])]
        public virtual void FF6_OptimumEquipmentForCompletelyUnequippedCharacter_(string slot, string item, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Optimum equipment for completely unequipped character.", exampleTags);
#line 110
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 111
 testRunner.When("I select Equip, Tora, Optimum", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 112
 testRunner.Then(string.Format("{0} is equipped on Tora in the {1} slot", item, slot), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="FF6 - Remove Items")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Remove Items")]
        [Xunit.InlineDataAttribute("R-hand", "Short Sword", new string[0])]
        [Xunit.InlineDataAttribute("L-hand", "Leather Shield", new string[0])]
        [Xunit.InlineDataAttribute("Head", "Leather Helm", new string[0])]
        [Xunit.InlineDataAttribute("Body", "Leather Armor", new string[0])]
        public virtual void FF6_RemoveItems(string slot, string item, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Remove Items", exampleTags);
#line 121
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 122
 testRunner.Given("Tora is equipped with Short Sword, Leather Shield, Leather Helm, Leather Armor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 123
 testRunner.And("the inventory is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.When(string.Format("I select Equip, Tora, Remove, {0}", slot), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 125
 testRunner.Then(string.Format("nothing is equipped on Tora in the {0} slot", slot), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 126
 testRunner.And(string.Format("{0} is in the inventory", item), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="FF6 - Empty Items")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Empty Items")]
        [Xunit.InlineDataAttribute("R-hand", "Short Sword", new string[0])]
        [Xunit.InlineDataAttribute("L-hand", "Leather Shield", new string[0])]
        [Xunit.InlineDataAttribute("Head", "Leather Helm", new string[0])]
        [Xunit.InlineDataAttribute("Body", "Leather Armor", new string[0])]
        public virtual void FF6_EmptyItems(string slot, string item, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Empty Items", exampleTags);
#line 135
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 136
 testRunner.Given("Tora is equipped with Short Sword, Leather Shield, Leather Helm, Leather Armor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 137
 testRunner.And("the inventory is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
 testRunner.When("I select Equip, Tora, Empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 139
 testRunner.Then(string.Format("nothing is equipped on Tora in the {0} slot", slot), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 140
 testRunner.And(string.Format("{0} is in the inventory", item), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Equip relic")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Equip relic")]
        public virtual void FF6_EquipRelic()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Equip relic", ((string[])(null)));
#line 149
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 150
 testRunner.When("I select relic, Tora, Equip, Relic 1, Sprint Shoes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 151
 testRunner.Then("Sprint Shoes is equipped on Tora in the relic 1 slot", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="FF6 - Exit relic menus")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Exit relic menus")]
        public virtual void FF6_ExitRelicMenus()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Exit relic menus", ((string[])(null)));
#line 153
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 154
 testRunner.When("I select Relic, Tora, Equip, Relic 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 155
 testRunner.And("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
 testRunner.Then("the active window is relic/slots", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 157
 testRunner.When("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 158
 testRunner.Then("the active window is relic/equipActionType", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 159
 testRunner.When("I press the cancel button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 160
 testRunner.Then("the active window is default/main", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="FF6 - Remove relic")]
        [Xunit.TraitAttribute("FeatureTitle", "FF6Menu")]
        [Xunit.TraitAttribute("Description", "FF6 - Remove relic")]
        [Xunit.InlineDataAttribute("Relic 1", "Sprint Shoes", new string[0])]
        [Xunit.InlineDataAttribute("Relic 2", "Running Shoes", new string[0])]
        public virtual void FF6_RemoveRelic(string slot, string item, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FF6 - Remove relic", exampleTags);
#line 162
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 163
 testRunner.Given("Tora is equipped with Sprint Shoes, Running Shoes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 164
 testRunner.And("the inventory is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
 testRunner.When(string.Format("I select relic, Tora, Remove, {0}", slot), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 166
 testRunner.Then(string.Format("{0} is in the inventory", item), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 167
 testRunner.And(string.Format("nothing is equipped on Tora in the {0} slot", slot), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                FF6MenuFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                FF6MenuFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
