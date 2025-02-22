using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using PhotoshopApp.DataSourceHandlers;

namespace PhotoshopApp.Models.Request;

/// <summary>
/// Request model for adding new item payload
/// </summary>
public class GetBerryRequest
{
    // Properties must have display attributes which contain user-friendly name of variable
    [Display("Berry name", Description = "The name of the berry")]
    // Applying data source handler to the property
    [DataSource(typeof(AsyncDataSourceHandler))]
    public string BerryName { get; set; }

    [Display("Pokemon type")]
    [StaticDataSource(typeof(StaticDataSourceHandler))]
    public string PokemonType { get; set; }
}