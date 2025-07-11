# Azure Deployment Guide for S186 Statements Web

## Prerequisites

1. **Azure Subscription** - Active Azure subscription
2. **Azure CLI** - Install and login: `az login`
3. **Strapi CMS** - Deployed and accessible from Azure
4. **Azure AD App Registration** - Configured for production

## Step 1: Prepare Azure Resources

### Create Resource Group

```bash
az group create --name s186-statements-rg --location "UK South"
```

### Create App Service Plan

```bash
az appservice plan create --name s186-statements-plan --resource-group s186-statements-rg --sku B1 --is-linux
```

### Create Web App

```bash
az webapp create --name s186-statements-web --resource-group s186-statements-rg --plan s186-statements-plan --runtime "DOTNETCORE:8.0"
```

## Step 2: Configure Azure AD for Production

1. **Update App Registration** in Azure Portal:

   - Go to Azure AD → App registrations → Your app
   - Add production redirect URI: `https://your-app-name.azurewebsites.net/signin-oidc`
   - Add production logout URI: `https://your-app-name.azurewebsites.net/signout-oidc`

2. **Update appsettings.Production.json** with your production values:
   ```json
   {
     "AzureAd": {
       "Domain": "your-actual-domain.onmicrosoft.com",
       "TenantId": "your-actual-tenant-id",
       "ClientId": "your-actual-client-id"
     }
   }
   ```

## Step 3: Configure Environment Variables

Set these in Azure App Service Configuration:

```bash
# Azure AD Configuration
az webapp config appsettings set --name s186-statements-web --resource-group s186-statements-rg --settings \
  "AzureAd__Domain=your-domain.onmicrosoft.com" \
  "AzureAd__TenantId=your-tenant-id" \
  "AzureAd__ClientId=your-client-id"

# Strapi Configuration
az webapp config appsettings set --name s186-statements-web --resource-group s186-statements-rg --settings \
  "Strapi__BaseUrl=https://your-strapi-production-url.com" \
  "Strapi__ApiToken=your-production-api-token"
```

## Step 4: Deploy the Application

### Option A: Deploy from Local Git

```bash
# Initialize git if not already done
git init
git add .
git commit -m "Initial commit"

# Add Azure remote
az webapp deployment source config-local-git --name s186-statements-web --resource-group s186-statements-rg

# Get the git URL and push
git remote add azure <git-url-from-above>
git push azure main
```

### Option B: Deploy from GitHub

1. Connect your GitHub repository in Azure Portal
2. Configure deployment settings
3. Enable continuous deployment

### Option C: Deploy using Azure CLI

```bash
# Build and publish locally
dotnet publish -c Release -o ./publish

# Deploy to Azure
az webapp deployment source config-zip --resource-group s186-statements-rg --name s186-statements-web --src ./publish.zip
```

## Step 5: Configure HTTPS and Custom Domain (Optional)

```bash
# Enable HTTPS
az webapp update --name s186-statements-web --resource-group s186-statements-rg --https-only true

# Add custom domain (if you have one)
az webapp config hostname add --webapp-name s186-statements-web --resource-group s186-statements-rg --hostname your-domain.com
```

## Step 6: Monitor and Troubleshoot

### Check Application Logs

```bash
az webapp log tail --name s186-statements-web --resource-group s186-statements-rg
```

### Common Issues and Solutions

1. **Authentication Errors**

   - Verify Azure AD configuration
   - Check redirect URIs match exactly
   - Ensure app registration has correct permissions

2. **Strapi Connection Errors**

   - Verify Strapi URL is accessible from Azure
   - Check API token is valid
   - Ensure CORS is configured in Strapi

3. **Build Errors**
   - Check .NET version compatibility
   - Verify all dependencies are included
   - Review build logs in Azure

## Environment Variables Reference

| Variable            | Description                | Example                                |
| ------------------- | -------------------------- | -------------------------------------- |
| `AzureAd__Domain`   | Your Azure AD domain       | `contoso.onmicrosoft.com`              |
| `AzureAd__TenantId` | Azure AD tenant ID         | `12345678-1234-1234-1234-123456789012` |
| `AzureAd__ClientId` | App registration client ID | `87654321-4321-4321-4321-210987654321` |
| `Strapi__BaseUrl`   | Strapi CMS URL             | `https://strapi.contoso.com`           |
| `Strapi__ApiToken`  | Strapi API token           | `your-api-token-here`                  |

## Security Best Practices

1. **Never commit secrets** - Use Azure Key Vault for sensitive data
2. **Enable HTTPS** - Always use HTTPS in production
3. **Configure CORS** - Restrict CORS to your domains
4. **Monitor access** - Set up Azure Application Insights
5. **Regular updates** - Keep dependencies updated

## Cost Optimization

- Use **B1** plan for development/testing
- Scale to **P1V2** or higher for production
- Enable **auto-scaling** for variable load
- Use **Azure CDN** for static content
