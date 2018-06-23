Feature: ps3 menu
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given I have the PS3 menu
#
#Scenario Outline: PS3 - Equip an item on
#	When I select equip
#	And I select pc 1
#	And I select <item>
#	And I select <slot>
#	Then <item> is equipped on pc 1 in the <slot> slot
#
#	Examples:
#	| slot   | item          |
#	| r-hand | long sword    |
#	| l-hand | long sword    |
#	| torso  | leather armor |
#	| feet   | leather boots |
#	| buckle | ruby buckle   |
#
#Scenario: PS3 - can't use item
#	When i select item
#	And i select pc 1
#	And i select item 1
#	And i select use (discard)
#	Then the first menu item is can't
#
#Scenario: PS3 - use a technique
#	When i select technique
#	And i select pc 2
#	And i select restore
#	And i select pc 1
#	Then magic happens
#
#Scenario: PS3 - stats
#	When i select stats and pc 1
#	Then the stats workspace is active
#
#Scenario: PS3 - switch characters
#	When i select switch
#	And i select pc 2
#	Then the menu exits
#
#
