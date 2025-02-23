using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.MarkdownApp;

public class Application : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.Utilities];
        set { }
    }

    public string Name
    {
        get => "Markdown App";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}