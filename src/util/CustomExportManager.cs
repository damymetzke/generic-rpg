using Godot;
using System.Collections.Generic;

class CustomExportManager
{
    public delegate object GetPropertyFunction();
    public delegate void SetPropertyFunction(object value);

    private Godot.Collections.Array propertyList = new Godot.Collections.Array();
    private Dictionary<string, GetPropertyFunction> getPropertyFunctions = new Dictionary<string, GetPropertyFunction>();
    private Dictionary<string, SetPropertyFunction> setPropertyFunctions = new Dictionary<string, SetPropertyFunction>();

    private List<string> currentGroup = new List<string>();

    public void RegisterCategory(string name)
    {
        var newCategory = new Godot.Collections.Dictionary();
        newCategory["name"] = "Player";
        newCategory["type"] = Godot.Variant.Type.Nil;
        newCategory["usage"] = Godot.PropertyUsageFlags.Category | Godot.PropertyUsageFlags.ScriptVariable;

        propertyList.Add(newCategory);
    }

    public void PushGroup(string name)
    {
        currentGroup.Add(name);
    }

    public void PopGroup()
    {
        currentGroup.RemoveAt(currentGroup.Count - 1);
    }

    public void RegisterProperty(string name, Godot.Variant.Type type, GetPropertyFunction getFunction, SetPropertyFunction setFunction)
    {
        // calculate name
        string propertyName = "";

        foreach (string group in currentGroup)
        {
            propertyName += $"{group}/";
        }

        propertyName += name;

        // add property to property list
        var newProperty = new Godot.Collections.Dictionary();
        newProperty["name"] = propertyName;
        newProperty["type"] = type;
        newProperty["usage"] = Godot.PropertyUsageFlags.Default;
        propertyList.Add(newProperty);

        // setup get and set logic
        getPropertyFunctions.Add(propertyName, getFunction);
        setPropertyFunctions.Add(propertyName, setFunction);

    }

    public Godot.Collections.Array GetPropertyList()
    {
        return propertyList;
    }

    public object GetProperty(string property)
    {
        if (!getPropertyFunctions.ContainsKey(property))
        {
            return null;
        }

        return getPropertyFunctions[property]();
    }

    public bool SetProperty(string property, object value)
    {
        if (!setPropertyFunctions.ContainsKey(property))
        {
            return false;
        }

        setPropertyFunctions[property](value);
        return true;
    }

}