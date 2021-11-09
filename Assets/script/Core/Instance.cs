namespace Platformer.Core
{
    public static partial class Instance<T> where T : class, new()
    {
	public static T instance = new T();
	public static T get()
	{
	    return instance;
	}
    }
}
