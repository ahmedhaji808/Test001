namespace AspnetCoreMvcFull.Domain.ValueObjects;

public readonly record struct ProjectId(Guid Value)
{
  public static ProjectId New() => new(Guid.NewGuid());

  public static bool TryParse(string? input, out ProjectId projectId)
  {
    if (Guid.TryParse(input, out var guid))
    {
      projectId = new ProjectId(guid);
      return true;
    }
    projectId = default;
    return false;
  }

  public override string ToString() => Value.ToString();
}