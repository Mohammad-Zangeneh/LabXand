namespace LabXand.SharedKernel;
[Flags]
public enum RestrictionTypes
{
    OnFetch = 0,
    OnAdd = 1,
    OnEdit = 2,
    OnDelete = 4
}

public enum ApplyRestrictionTypes
{
    None,
    Custom,
    All
}