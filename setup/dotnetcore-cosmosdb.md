#Create CosmosDB Data Store
The dotnetcore project requires access to a CosmosDB instance running in an Azure Subscription.  These steps walkthrough the creation of this repository and configuration of the project.

Parameters - use this space to define the variables used when creating the CosmosDB.  Blank spaces require a unique value for your project.  Completed items are the defaults for the project.  If you change these, you will need to update the `Settings.cs` with the new values.

Resource Group - 
Account Name - 
Database Name - "ReadingList"
Collection Name - "Recommendations"
UserName - "testuser"

**Create Using Azure-CLI**
Refer to the steps in the [Connect to Azure-CLI](https://github.com/Microsoft/MTC_ContainerCamp/blob/master/setup/xplat-cli-login.md) to authenticate and connect to your subscription. 

**Create the CosmosDB**
First we need a resource group to place our Azure components.
```
az group create -n myResourceGroupName --location eastus
```

Now that we have the group, let's create the CosmosDB.
```
az cosmosdb create -n accountname -g myResourceGroupName ---kind GlobalDocumentDB
```

Create the Database in our CosmosDB
```
az cosmosdb database create -d "ReadingList" -n accountname -g myResourceGroupName
```

You need a collection to place your content.  Create the collection in the new database.
```
az cosmosdb collection create -c "Recommendations" -d "ReadingList" -n accountname -g myResourceGroupName
```

You will need values now that everything is created.  First get the DocumentEndpoint value.  You can get this by running.
```
az cosmosdb show -n accountname -g myResourceGroupName --output table
```
Copy this value to use in the docker-compose file below.

Next get the Primary Master Key.  This is available from the following command.
```
az cosmosdb list-keys -n accountname - g myResourceGroupName
```
Copy the Primary Master Key value to use in the docker-compose file below.

**Create the documents in the collection**
Documents are placed in the new collection through the web app.  To get the web app up and running, you need to add the values into the docker-compose file.
The docker-compose.yml is in the docker-compose project.  In this file is a section for environment.  Here you will find space to provide the values created from the above steps.
Save the modified docker-compose and run the project.

Once the web page loads, click the Setup link in the menu bar.  In the Setup page, click the Deploy Default Documents button.  This will place a number of documents into the CosmosDB database and collection created above.    
