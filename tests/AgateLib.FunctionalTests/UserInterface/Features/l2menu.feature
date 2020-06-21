Feature: l2menu
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given I have the L2Menu
#
## item      status
## spell     change
## capsule   config
## equip     scenario
#
#Scenario: L2 - use an item
#	When i select item
#	And i select item 1 twice
#	And i select pc 1
#	Then pc 1 is healed
#
#Scenario: L2 - drop an item
#	When i select item
#	And i push cancel
#	And i select drop
#	And i select item 1 twice
#	Then item 1 is gone
#
#Scenario: L2 - swap items
#	When I select item
#	And i select item 1
#	And i select item 2
#	Then item 2 is in position 1
#
#Scenario: L2 - sort items
#	When i select item
#	And i push cancel
#	And i select sort
#	Then the items are sorted
#
#Scenario: L2 - status
#	When i select status
#	And i select pc 1
#	Then the status workspace for pc 1 is active
#
#Scenario: L2 - status 2
#	When i select status
#	And i select pc 1
#	And i select next
#	Then the status workspace for pc 2 is active
#
#Scenario: L2 - exit status
#	When i select status
#	And i select pc 1
#	And i select return
#	Then the default workspace is active
#
#Scenario: L2 - cast spell
#	When i select spell
#	And i select pc 1
#	And i select heal twice
#	And i select pc 2
#	Then pc 2 is healed
#
#Scenario: L2 - sort spells
#	When i select spell
#	And i push cancel
#	And i select sort
#	Then the items are sorted
#
#Scenario: L2 - swap spells
#	When I select spells
#	And i select item 1
#	And i select item 2
#	Then item 2 is in position 1
#
#Scenario: L2 - change message speed
#	When i select config
#	And i push right twice
#	Then the message speed is slow (right once for normal)
#
#Scenario: L2 - change battle curor
#	When i select config
#	And i push left once
#	Then the battle cursor is clear
#
#Scenario: L2 - equip an item
#	When i select equip
#	And i select pc 1
#	And I select equip
#	And i select short sword
#	And i select long sword
#	Then the long sword is equipped in slot 1 on pc 1
#
#Scenario: L2 - equip strongest
#	When i select equip
#	And i select pc 1
#	And i select strongest
#	Then the long sword is equipped in slot 1 on pc 1
#
#Scenario: L2 - unequip item
#	When i select equip
#	And i select pc 1
#	And I select remove
#	And i select short sword
#	Then pc 1 equipment slot 1 is empty
#	And the short sword is in the inventory
#
#Scenario: L2 - unequip all items
#	When i select equip
#	And i select pc 1
#	And i select remove all
#	Then pc 1 equipment slots are all empty
#	And the short sword is in the inventory
#
#Scenario: L2 - drop from equipment
#	When i select equip
#	And i select pc 1
#	And i select drop
#	And i select short sword
#	Then pc 1 equipment slot 1 is empty
#	And the short sword is not in the inventory
#
#
