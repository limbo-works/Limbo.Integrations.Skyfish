using System.Collections.Generic;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Collections;
using Skybrud.Essentials.Http.Options;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Integrations.Skyfish.Options.Folders;

/// <summary>
/// Class representing the options for searching through Skyfish folders.
/// </summary>
public class SkyfishSearchFoldersOptions : IHttpRequestOptions {

    #region Properties

    /// <summary>
    /// Query string matching against the folder name.
    /// </summary>
    public string? Query { get; set; }

    /// <summary>
    /// Limit the list to the folder(s) that has the given ID(s).
    /// </summary>
    public List<int> Ids { get; set; } = new();

    /// <summary>
    /// Limit the list to folders with the parent ID specified.
    /// </summary>
    public int? Parent { get; set; }

    /// <summary>
    /// Limit the list to folders owned by the company. Default behaviour.
    /// </summary>
    public int? CompanyId { get; set; }

    /// <summary>
    /// Specifies which field you would like to sort by, currently <see cref="SkyfishFolderSortField.Name"/> and
    /// created is <see cref="SkyfishFolderSortField.Created"/>.
    /// </summary>
    public SkyfishFolderSortField? SortBy { get; set; }

    /// <summary>
    /// Can be either <see cref="SkyfishSortOrder.Ascending"/>, <see cref="SkyfishSortOrder.Descending"/> or left out
    /// for <see cref="SkyfishSortOrder.Ascending"/>.
    /// </summary>
    public SkyfishSortOrder? SortOrder { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance with default options.
    /// </summary>
    public SkyfishSearchFoldersOptions() { }

    #endregion

    #region Member methods

    /// <inheritdoc />
    public IHttpRequest GetRequest() {

        // Initialize the query string
        IHttpQueryString query = new HttpQueryString();

        // Append optional parameters if specified
        if (!string.IsNullOrWhiteSpace(Query)) query.Add("q", Query!);
        if (Ids is { Count: > 0 }) query.Add("id", string.Join(" ", Ids));
        if (Parent is not null) query.Add("parent", Parent);
        if (CompanyId is not null) query.Add("company_id", CompanyId);
        if (SortBy is not null) query.Add("sort_by", SortBy.Value.ToLower());
        if (SortOrder is not null) query.Add("sort_order", SortOrder.Value == SkyfishSortOrder.Ascending ? "asc" : "desc");

        // Initialize a new GET request
        return HttpRequest.Get("/folder", query);

    }

    #endregion

}