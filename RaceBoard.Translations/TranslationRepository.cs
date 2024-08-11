using Dapper;
using RaceBoard.Translations.Entities;
using RaceBoard.Translations.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace RaceBoard.Translations
{
    public class TranslationRepository : ITranslationRepository
    {
        #region Private Members

        private string _connectionString;

        #endregion

        #region Constructors

        public TranslationRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString_Translations"];
        }

        #endregion

        #region IRepository implementation

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        #endregion

        #region ITranslationRepository implementation

        public Translation Get(string key, string? language = null)
        {
            return this.GetTranslations(key: key, language: language).FirstOrDefault();
        }

        public List<Translation> Get(string? language = null)
        {
            return this.GetTranslations(key: null, language: language);
        }

        #endregion

        #region Private Methods

        private List<Translation> GetTranslations(string? key = null, string? language = null)
        {
            StringBuilder query = GetTranslationQuery();

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(key))
            {
                query.AppendLine($" {GetLogicOperator(parameters)} [Tag].[Name] = @name");
                parameters.Add("name", key.ToLower());
            }

            if (!string.IsNullOrEmpty(language))
            {
                //query.AppendLine($" {GetOperator(parameters)} [Translation].IdLanguage = @idLanguage");
                //parameters.Add("idLanguage", language);

                query.AppendLine($" {GetLogicOperator(parameters)} SUBSTRING([Language].IsoCode, 1, 2) = @language");
                parameters.Add("language", language.ToLower());
            }

            query.AppendLine(" ORDER BY [Tag].[Name] ASC, [Language].Id ASC");

            List<Translation> translations = null;

            var dictonary = new Dictionary<string, Translation>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Query<Translation, TranslatedText, Language, Translation>
                (
                    query.ToString(),
                    (translation, translatedText, language) =>
                    {
                        if (!dictonary.ContainsKey(translation.Key))
                        {
                            dictonary.Add(translation.Key, translation);
                        }

                        //translatedText.Language = language.Name;
                        translatedText.Language = language.IsoCode;

                        dictonary[translation.Key].Translations.Add(translatedText);

                        return translation;
                    },
                    param: parameters,
                    splitOn: "Id, Id, Id"
                ).AsList();
            }

            return dictonary.Values.Select(x => x).ToList();
        }

        private StringBuilder GetTranslationQuery()
        {
            var query = new StringBuilder();

            query.Append
                (@"
                    SELECT
	                    [Tag].[Id]              [Id],
	                    [Tag].[Name]            [Key],
	                    [Translation].Id        [Id],
	                    [Translation].[Text]    [Text],
	                    [Language].Id           [Id],
	                    [Language].Name         [Name],
                        [Language].IsoCode      [IsoCode]
                    FROM [Translation] [Translation]
                    LEFT JOIN [Tag] [Tag] ON [Tag].Id = [Translation].IdTag
                    LEFT JOIN [Language] [Language] ON [Language].Id = [Translation].IdLanguage"
                );

            return query;
        }

        private string GetLogicOperator(DynamicParameters parameters)
        {
            return parameters.ParameterNames.Any() ? " AND " : " WHERE ";

        }

        #endregion
    }

}