Feature: FF6Menu

Background:
	Given I have the FF6 system
	Given a party of 
	| Name    | HP  | MaxHP |
	| Tora    | 70  | 100   |
	| Unlocke | 100 | 100   |
	| Deadgar | 100 | 100   |
	| Sabo    | 100 | 100   |
	And an inventory of
	| Name           | ItemType | Effect   |
	| Short Sword    | Weapon   |          |
	| Long Sword     | Weapon   |          |
	| Leather Shield | Shield   |          |
	| Leather Armor  | Armor    |          |
	| Leather Helm   | Helm     |          |
	| Sprint Shoes   | Relic    |          |
	| Running Shoes  | Relic    |          |
	| White Cape     | Relic    |          |
	| Dried Meat     | Item     | heal 999 |
	And I open the menu

Scenario: FF6 - Open Items Menu
	When I select Items
	Then the active window is items/items

Scenario: FF6 - Use Item
	When I select Items, Dried Meat, Dried Meat, Tora
	Then Tora is healed

Scenario: FF6 - Reorder items manually
	When I select Items
	And I press the down button
	And I press the accept button
	Then Long Sword is the active menu item
	When I press the down button 2 times
	And I press the accept button
	Then Leather Armor is in slot 1 in the inventory
	And Long Sword is in slot 3 in the inventory

Scenario: FF6 - Open Skills Menu
	When I select Skills, Tora
	Then the active window is skills/skillType

#Scenario: FF6 - Exiting menu raises exit event
#	When I press the exit button
#	Then the exit event is triggered

Scenario: FF6 - Open Items Menu and Return to Main Menu
	When I select Items
	And I press the cancel button 2 times
	Then the default workspace is active
	And the active window is default/main

#Scenario: FF6 - Activate Rare Items window
#	When I select Items
#	And I press the cancel button
#	And I select rare
#	Then the rare items workspace is active

Scenario: FF6 - Arrange items
	When I select Items
	And I press the cancel button
	And I select Arrange
	Then the items are arranged

Scenario: FF6 - Activate magic
	When I select Skills, Tora, Magic
	Then the magic workspace is active
	When I press the cancel button 2 times
	Then the active window is default/main

Scenario: FF6 - Activate Espers
	When I select Skills, Tora, Espers
	Then the Espers workspace is active

Scenario Outline: FF6 - Equip Items
	When I select Equip, Tora, Equip, <slot>, <item>
	Then <item> is equipped on Tora in the <slot> slot
	
	Examples:
	| slot   | item           |
	| R-hand | Short Sword    |
	| L-hand | Leather Shield |
	| Head   | Leather Helm   |
	| Body   | Leather Armor  |
	
Scenario: FF6 - Cancel PC selection after equip
	When I select Equip
	And I press the cancel button
	Then the active window is default/main

Scenario: FF6 - Exit equip menus
	When I select Equip, Tora, Equip, R-hand
	And I press the cancel button
	Then the active window is equip/slots
	When I press the cancel button
	Then the active window is equip/equipActionType
	When I press the cancel button
	Then the active window is default/main

Scenario: FF6 - Equip multiple items in succession
	When I select Equip, Tora, Equip, R-hand, Short Sword, L-hand, Leather Shield, Head, Leather Helm, Body, Leather Armor
	Then Short Sword is equipped on Tora in the R-hand slot
	Then Leather Shield is equipped on Tora in the L-hand slot
	Then Leather Helm is equipped on Tora in the Head slot
	Then Leather Armor is equipped on Tora in the Body slot
	
Scenario Outline: FF6 - Optimum equipment for completely unequipped character.
	When I select Equip, Tora, Optimum
	Then <item> is equipped on Tora in the <slot> slot
	
	Examples:
	| slot   | item           |
	| R-hand | Short Sword    |
	| L-hand | Leather Shield |
	| Head   | Leather Helm   |
	| Body   | Leather Armor  |

Scenario Outline: FF6 - Remove Items
	Given Tora is equipped with Short Sword, Leather Shield, Leather Helm, Leather Armor
	And the inventory is empty
	When I select Equip, Tora, Remove, <slot>
	Then nothing is equipped on Tora in the <slot> slot
	And <item> is in the inventory
	
	Examples:
	| slot   | item           |
	| R-hand | Short Sword    |
	| L-hand | Leather Shield |
	| Head   | Leather Helm   |
	| Body   | Leather Armor  |
	
Scenario Outline: FF6 - Empty Items
	Given Tora is equipped with Short Sword, Leather Shield, Leather Helm, Leather Armor
	And the inventory is empty
	When I select Equip, Tora, Empty
	Then nothing is equipped on Tora in the <slot> slot
	And <item> is in the inventory
	
	Examples:
	| slot   | item           |
	| R-hand | Short Sword    |
	| L-hand | Leather Shield |
	| Head   | Leather Helm   |
	| Body   | Leather Armor  |

Scenario: FF6 - Equip relic
	When I select relic, Tora, Equip, Relic 1, Sprint Shoes
	Then Sprint Shoes is equipped on Tora in the relic 1 slot

Scenario: FF6 - Exit relic menus
	When I select Relic, Tora, Equip, Relic 1
	And I press the cancel button
	Then the active window is relic/slots
	When I press the cancel button
	Then the active window is relic/equipActionType
	When I press the cancel button
	Then the active window is default/main

Scenario Outline: FF6 - Remove relic
	Given Tora is equipped with Sprint Shoes, Running Shoes
	And the inventory is empty
	When I select relic, Tora, Remove, <slot>
	Then <item> is in the inventory
	And nothing is equipped on Tora in the <slot> slot

	Examples:
	| slot    | item          |
	| Relic 1 | Sprint Shoes  |
	| Relic 2 | Running Shoes |
