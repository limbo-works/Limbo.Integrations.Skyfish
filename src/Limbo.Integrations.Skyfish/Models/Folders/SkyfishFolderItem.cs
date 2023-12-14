using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;
using Skybrud.Essentials.Time;

namespace Limbo.Integrations.Skyfish.Models.Folders {

    /// <summary>
    /// Class representing a Skyfish folder item.
    /// </summary>
    public class SkyfishFolderItem : JsonObjectBase {

        #region Properties

        /// <summary>
        /// Gets the ID of the folder.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name of the folder.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the folder ID of the parent folder. <see langword="null"/> if no parent folder exists.
        /// </summary>
        public int? Parent { get; }

        /// <summary>
        /// Gets the ID of the company.
        /// </summary>
        public int CompanyId { get; }

        /// <summary>
        /// Gets the time the folder was created.
        /// </summary>
        public EssentialsTime Created { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The JSON object representing the folder.</param>
        protected SkyfishFolderItem(JObject json) : base(json) {
            Id = json.GetInt32("id");
            Name = json.GetString("name")!;
            Parent = json.GetInt32OrNull("parent");
            CompanyId = json.GetInt32("company_id");
            Created = json.GetString("created", EssentialsTime.Parse)!;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SkyfishFolderItem"/> based on the specified <paramref name="json"/> object.
        /// </summary>
        /// <param name="json">The JSON object representing the folder item.</param>
        /// <returns>An instance of <see cref="SkyfishFolderItem"/>.</returns>
        [return: NotNullIfNotNull("json")]
        public static SkyfishFolderItem? Parse(JObject? json) {
            return json == null ? null : new SkyfishFolderItem(json);
        }

    }

}