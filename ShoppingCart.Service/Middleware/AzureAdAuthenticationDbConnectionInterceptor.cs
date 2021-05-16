﻿using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ShoppingCart.Service.Middleware
{
    public class AzureAdAuthenticationDbConnectionInterceptor : DbConnectionInterceptor
    {
        // See https://docs.microsoft.com/azure/active-directory/managed-identities-azure-resources/services-support-managed-identities#azure-sql
        private static readonly string[] _azureSqlScopes = new[]
        {
        "https://database.windows.net//.default"
    };

        private static readonly TokenCredential _credential =
            new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ExcludeInteractiveBrowserCredential = false,
                ExcludeVisualStudioCredential = true,
            });

        public override InterceptionResult ConnectionOpening(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result)
        {
            var sqlConnection = (SqlConnection)connection;
            if (DoesConnectionNeedAccessToken(sqlConnection))
            {
                var tokenRequestContext = new TokenRequestContext(_azureSqlScopes);
                var token = _credential.GetToken(tokenRequestContext, default);

                sqlConnection.AccessToken = token.Token;
            }

            return base.ConnectionOpening(connection, eventData, result);
        }

        public override async Task<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            var sqlConnection = (SqlConnection)connection;
            if (DoesConnectionNeedAccessToken(sqlConnection))
            {
                var tokenRequestContext = new TokenRequestContext(_azureSqlScopes);
                var token = await _credential.GetTokenAsync(tokenRequestContext, cancellationToken);

                sqlConnection.AccessToken = token.Token;
            }

            return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }

        private static bool DoesConnectionNeedAccessToken(SqlConnection connection)
        {
            //
            // Only try to get a token from AAD if
            //  - We connect to an Azure SQL instance; and
            //  - The connection doesn't specify a username.
            //
            var connectionStringBuilder = new SqlConnectionStringBuilder(connection.ConnectionString);

            return connectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(connectionStringBuilder.UserID);
        }
    }
}