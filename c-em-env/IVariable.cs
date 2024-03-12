namespace c_em_env
{
    /// <summary>
    /// Represents an interface for accessing dynamic variables.
    /// </summary>
    public interface IVariable
    {
        string? GetDynamicValue(string key);
        dynamic this[string key] { get; }
        dynamic AsDynamic();
    }
}
