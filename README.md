# Modifiers extension
Easily make flexible parameters for your characters, items, abilities, buffs in Unity.

## Use cases
Your game character has some parameters, for example - move speed, max health, etc.

You need to add possibility to affect these params - potions buffs, which will increase values of character parameters; enemies attacks debuffs, which will decrease these values, etc. And all of these should be combined easily.

So, there you can use this extension. 


## How it works
It is just replaces basic variables to a new thing called Modifier Param (wip naming). It can be max health, damage, move speed - any other thing. These values can be added to params group, which is simple called Modifier. This group describes params of some object - character, buff, item, etc.

There is a **Modifier** class, which contains of user-defined parameter types, you can flexible change it. Every buff or other thing has its own Modifier. Modifier is a simple ScriptableObject, so every modifier is just an asset in project folder.

There also **Modifiable** class, which you can add for your character/weapon/etc. It contains basic params of this object (something like default Modifier), and can be affected by adding/removing other modifiers from code. Modifable is a MonoBehaviour, it can be added to your objects by drag n drop.

You can access Modifiable parameters with a simple API and get values of your params - it automatically will summarize all modifiers, added to this object, and return final value, actual for this param.

All these parameters can be edited or added/removed at all without any code and project recompile. 

**Modifiers work example:**

Character move speed without modifiers is **3**.

Some speed up buff adds extra **2** speed.

But also there is some debuff which really slows down character, its speed value **-4**.

So, final value (returned by the API) of character speed is **1** (3 + 2 + (-4)).

## Why just not to use OOP and variables
Modifiers extension allows you to:
1. Unified serialization.
2. Ready-made Sum and Subtract operations for the params.
3. Easily get values from GameObject, there is no need to use GetComponent or something other.
4. Easy to add new or remove old ones with minimum refactoring amount, which can be easily done with modern IDEs.
5. Easily move some of your gameplay systems between projects, since Modifiers allow your code to not depend on other game classes (check the example for more info).

It is works nice with MonoBehaviour approach, in other cases you may prefer your own implementation. Also, there are still some limitations etc - for example, if your game have stats for different types of objects (items, characters, etc), it is not very comfy to work with them in one Enum.

## How to install
First of all, you need install **dependencies**:
- https://github.com/mackysoft/Unity-SerializeReferenceExtensions.git?path=Assets/MackySoft/MackySoft.SerializeReferenceExtensions#1.1.9

Next, you can install this package by using Package Manager:
https://github.com/OlegDzhuraev/Modifiers.git

Alternative install: download repo as source code, unpack to your project. Also, you can check **Releases** for unitypackage.

## Quick start
### Setup steps
1. On startup window should appear with request to create **Unity Modifiers Settings**. Also you can manually create it by Right click context menu in the **Project Window** (or take it from the **Example** folder).
2. Add some values into its **Supported Params** list.
3. To use it from code with more comfort, you need to generate constants (all examples use generated constants, so, better to do it). It can be found in the Top Menu -> Tools -> Insane One Modifiers -> Common... -> **Generate constsants button**. It will generate the **ModType** static class with a list of constants, similar to your strings list in the Settings asset. *It is not necessary to generate it.*

### Adding a new modifier types.
Insert them to the **Supported Params** list of the **Unity Modifiers Settings** asset. Example values can be removed.

Don't forget about constants generation, if you're planning to use them.

#### How to rename already exist modifiers types
It can look a kinda tricky comparing string type names to, for example, Enum usage, but it is easy with modern IDE. You need to:
1. Replace string type name in all assets (Prefabs and SO), using your IDE. Replace all the **"OldName"** to the **"NewName"**. Note that quotes means strings, so include it in your replace request.
2. (only if you're using generated constants) rename the **OldName** constant to the **NewName** using your IDE. It will replace all code usages.
3. (fully optional, also only if you're using generated constants) Generate new constants to approve that you renamed all correct.

### How to create a new modifier?
Right click in the Project window -> Modifiers -> **New modifier**.

Next, you're need to add required params to the list in this modifier. It can be Health, Damage, etc.

### How to create buff?
Buffs are modifiers with a lifetime and stacks amount parameter. It is just simplifies creation of this functionality.

Right click in the Project window -> Modifiers -> **New buff**.

Setup buff parameters, add buff modifiers.

You can create your own buff class (derived from Buff) and add there your info, it can be icon, text id or something other.
## Code usage

In order to use this package, you need to add scripting define symbol `INSANEONE_MODIFIERS_EXTENSION` to the **Project Settings**. 
It enables extension methods for GameObject class and ModifiableBehaviour class, used as shortcut to modifiers methods.

You also can skip this and use package without this define, but it not so handy and examples will not work.

### Adding and removing modifiers
This is one of the most common actions. You add and remove stats for your characters etc regularly.
```cs 
[SerializeField] Modifier modifier;

void Start() 
{
  // This and below code uses custom extensions methods
    
  // Add
  gameObject.AddModifier(modifier);
    
  // Remove (note that if you instance modifier, by remove method you will be able to remove only modifiers, same to instance, not the original one).
  gameObject.RemoveModifier(modifier);
}
```

### Get actual value
This is another most common action. To handle any values, you need to get them somehow. This example shows, how it's done in this extension.
```cs
// Getting value of Max Health param
gameObject.GetModifierValue(ModType.MaxHealth);
```

### Tags
If you need to tag object and check if object have some tags, you can use these methods.
Tags are just simple numbers like other stats, but these methods allows to work with them easily and make code more short.

```cs
gameObject.AddTag(ModType.TagA);

if (gameObject.HasTag(ModType.TagA))
{
  // do something
}

// checking for multiple tags, AND condition example
if (gameObject.HasAllTags(ModType.TagA, ModType.TagB))
{
  // do something if there all on the object
}

// OR condition example
if (gameObject.HasAnyTags(ModType.TagA, ModType.TagB))
{
  // do something if there at least one tag of the listed above.
}
```


### Make Filter and get results
```cs
// making filter which will filter all objects with team 1

Filter filter;

void Start() 
{
  filter = Filter.Make(ModType.Team, 1);
}

void Update() 
{
  var results = filter.GetResults();
  
  foreach (GameObject teamedGo in results)
  {
    // you can do something with result objects
  }
}
```

Other usage examples can be found in the **Example** package.

## Example
You can find it in the **Example** package, there is a sample scene.
It shows simple character with Max Health, Regeneration and Defense parameters.
Also, there is a "damager" which have Damage and Critical Chance parameters. Damage will be applied to the character on left mouse button click. Critical chance will be used to make x2 damage, if random value hits the chance.

## Showcase
Battleproofed in the **[Echo Storm](https://store.steampowered.com/app/2282200/Echo_Storm)** game.

## License
MIT
