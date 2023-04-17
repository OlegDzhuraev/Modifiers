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

All these parameters can be edited or added/removed at all without any code and project recompile. 

**Modifiers work example:**

Character move speed without modifiers is **3**.

Some speed up buff adds extra **2** speed.

But also there is some debuff which really slows down character, its speed value **-4**.

So, final value (returned by the API) of character speed is **1** (3 + 2 + (-4)).

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
Right click in the Project window -> Modifiers -> New buff.

Setup buff parameters, add buff modifiers.

You can create your own buff class (derived from Buff) and add there your info, it can be icon, text id or something other.
## Code usage
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
This is another most common action. To handle any values, you need to get them somehow. This example shows, how it done in this extension.
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

Other usage examples can be found in the Example folder.

## Example
You can find it in the Example folder, there is a sample scene.
It shows simple character with Max Health, Regeneration and Defense parameters.
Also there is a "damager" which have Damage and Critical Chance parameters. Damage will be applied to the character on left mouse button click. Critical chance will be used to make x2 damage, if random value hits the chance.

## License
MIT
