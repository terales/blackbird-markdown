using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace PhotoshopApp.DataSourceHandlers;

/// <summary>
/// Data source handler for static input values
/// Fetches static data, can be used e.g. for enums
/// Implements IStaticDataSourceItemHandler interface
/// </summary>
public class StaticDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("water_electric", "Water/Electric"),
        new("fighting_psychic", "Fighting/Psychic"),
        new("grass_flying", "Grass/Flying"),
    ];
}