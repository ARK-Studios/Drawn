using System;


// TODO: Create an Enum defining different shared variables types, ie. Per Class, Per Object group, ...
public enum InterfaceFieldSharing
{
    None,   // This variable is not shared, all instances need to be set independantly
    Class,  // All instances of this class in the curernt prefab share this variable
    Prefab, // This variable is shared with anyone referencing it in the prefab
}


[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class InterfaceFieldAttribute : Attribute
{
    private InterfaceFieldSharing sharing;
    private string shareID;
    private string description;

    public InterfaceFieldAttribute()
    {
        this.sharing = InterfaceFieldSharing.None;
        this.shareID = "";
        this.description = "";
    }

    public virtual bool Shared
    {
        get
        {
            if (this.shareID == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public virtual InterfaceFieldSharing Sharing
    {
        get { return sharing; }
        set { sharing = value; }
    }

    public virtual string ShareID
    {
        get { return shareID; }
        set { shareID = value; }
    }

    public virtual string Description
    {
        get { return description; }
        set { description = value; }
    }
}