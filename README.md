# Modifiers extension
Easily make flexible parameters for your characters, items, abilities, buffs in Unity.

## Use cases
Your game character has some parameters, for example - move speed, max health, etc.

You need to add posibility to affect these params - potions buffs, which will increase values of character parameters; enemies attacks debuffs, which will decrease these values, etc. And all of these should be combined easily.

So, there you can use this extension. 

## How it works
It is just replaces basic variables to a new thing called Modifier Param (wip naming). It can be max health, damage, move speed - any other thing. These values can be added to params group, which is simple called Modifier. This group describes params of some object - character, buff, item, etc.

There is a **Modifier** class, which contains of user-defined parameter types, you can flexible change it. Every buff or other thing has its own Modifier. Modifier is a simple ScriptableObject, so every modifier is just an asset in project folder.

There also **Modifable** class, which you can add for your character/weapon/etc. It contains basic params of this object (something like default Modifier), and can be affected by adding/removing other modifiers from code. Modifable is a MonoBehaviour, it can be added to your objects by drag n drop.

You can access Modifable parameters with a simple API and get values of your params - it automatically will summarize all modifiers, added to this object, and return final value, actual for this param.

**Example:**
Character move speed is 3.
Some speed up buff adds extra 2 speed.
But also there some debuff which really slows down character, its speed value -4.
So, final value (returned by an API) of character speed is 3 + 2 + (-4) == 1.

All these parameters can be edited or added/removed at all without any code and project recompile. 

## How to install
Download repo as source code, unpack to your project. Also you can check Releases for unitypackage.

Package manager install will not be supported on this repo.

## Quick start

### Adding a new modifier types.
Insert it to enum **ModifierType**, add new ones (remove default values, it is just for example).

### How to create modifier?
Right click in the Project window -> Modifiers -> New modifier.

Next, you're need to add required params to the list in this modifier.

### How to create buff?
Coming soon.

## Example
You can find it in the Example folder, there is a sample scene.
It shows simple character with Max Health, Regeneration and Defense parameters.
Also there is a "damager" which have Damage and Critical Chance parameters. Damage will be applied to the character on left mouse button click. Critical chance will be used to make x2 damage, if random value hits the chance.

## License
MIT
