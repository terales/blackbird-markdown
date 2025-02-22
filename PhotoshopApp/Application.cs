using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace PhotoshopApp;

public class Application : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.Utilities];
        set { }
    }

    public string Name
    {
        get => "Photoshop App";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}