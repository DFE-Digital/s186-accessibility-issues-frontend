# S186 Statements Web Application

An ASP.NET Core MVC web application for managing S186 accessibility statements, integrated with a Strapi CMS backend.

## Features

- **Microsoft Entra (Azure AD) Authentication**: Single sign-on for internal users
- **Service Management**: Create, read, update, and delete services
- **Issue Tracking**: Manage accessibility issues for services
- **Dashboard**: Overview of services and issues with key metrics
- **Strapi CMS Integration**: Full CRUD operations via REST API

## Prerequisites

- .NET 8.0 SDK
- Strapi CMS running (see s186-statement-service)
- Microsoft Entra (Azure AD) tenant configured

## Setup Instructions

### 1. Configure Microsoft Entra Authentication

1. Register a new application in your Microsoft Entra tenant
2. Configure the following settings:

   - **Redirect URI**: `https://localhost:7001/signin-oidc` (for development)
   - **Logout URI**: `https://localhost:7001/signout-oidc`
   - **Supported account types**: Accounts in this organizational directory only

3. Update `appsettings.json` with your Azure AD configuration:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "your-domain.onmicrosoft.com",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-id",
    "CallbackPath": "/signin-oidc",
    "SignedOutCallbackPath": "/signout-oidc"
  }
}
```

### 2. Configure Strapi Integration

1. Ensure your Strapi CMS is running (typically on `http://localhost:1337`)
2. Generate an API token in Strapi admin panel
3. Update `appsettings.json` with Strapi configuration:

```json
{
  "Strapi": {
    "BaseUrl": "http://localhost:1337",
    "ApiToken": "your-strapi-api-token"
  }
}
```

### 3. Run the Application

1. Navigate to the project directory:

   ```bash
   cd S186Statements.Web
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Run the application:

   ```bash
   dotnet run
   ```

4. Open your browser and navigate to `https://localhost:7001`

## Project Structure

```
S186Statements.Web/
├── Controllers/           # MVC Controllers
│   ├── HomeController.cs  # Dashboard and home page
│   ├── ServicesController.cs  # Service management
│   └── IssuesController.cs    # Issue management
├── Models/               # Data models matching Strapi schema
│   ├── Service.cs
│   ├── Issue.cs
│   ├── StatementTemplate.cs
│   ├── StatementSetting.cs
│   ├── ServiceUrl.cs
│   ├── IssueComment.cs
│   └── User.cs
├── Services/             # Business logic and API integration
│   ├── IStrapiApiService.cs
│   └── StrapiApiService.cs
├── Views/                # Razor views
│   ├── Home/
│   ├── Services/
│   └── Shared/
└── wwwroot/             # Static files
```

## Data Models

The application includes the following data models that correspond to the Strapi CMS schema:

- **Service**: Core entity with serviceId, name, and fipsId
- **Issue**: Accessibility issues with title, description, state, and metadata
- **StatementTemplate**: Templates for accessibility statements
- **StatementSetting**: Configuration settings for services
- **ServiceUrl**: URLs associated with services
- **IssueComment**: Comments on issues
- **User**: User management (from Strapi users-permissions)

## API Integration

The application communicates with the Strapi CMS through the `StrapiApiService`, which provides:

- Full CRUD operations for all entities
- Proper error handling and logging
- Authentication via API tokens
- JSON serialization/deserialization

## Security

- **Authentication**: Microsoft Entra (Azure AD) single sign-on
- **Authorization**: All controllers require authentication
- **API Security**: Strapi API token authentication
- **HTTPS**: Enforced in production

## Development

### Adding New Features

1. **New Entity**: Add model, service methods, controller, and views
2. **New API Endpoint**: Extend `IStrapiApiService` and `StrapiApiService`
3. **New View**: Create Razor view with proper model binding

### Testing

1. Ensure Strapi CMS is running
2. Configure authentication settings
3. Test CRUD operations for each entity
4. Verify authentication flow

## Deployment

### Production Configuration

1. Update `appsettings.Production.json` with production settings
2. Configure HTTPS certificates
3. Set up proper Azure AD application registration
4. Configure Strapi production URL and API token

### Environment Variables

Consider using environment variables for sensitive configuration:

```bash
export AzureAd__TenantId="your-tenant-id"
export AzureAd__ClientId="your-client-id"
export Strapi__ApiToken="your-api-token"
```

## Troubleshooting

### Common Issues

1. **Authentication Errors**: Verify Azure AD configuration
2. **API Connection Errors**: Check Strapi URL and API token
3. **CORS Issues**: Ensure Strapi CORS settings allow your domain

### Logging

The application uses structured logging. Check logs for detailed error information:

```bash
dotnet run --environment Development
```

## Contributing

1. Follow C# coding conventions
2. Add proper error handling
3. Include validation attributes on models
4. Test all CRUD operations
5. Update documentation as needed

## License

This project is for internal use only.
