## Azure CLI 2.0 Install Options & Login To Azure

1. You can access Azure via Azure-cli with any of the following options:
    
    Option 1. Install the lastest version of the Azure CLI.  Go to the [Install Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) page and follow the instructions for your host OS.
    
    Option 2: You can also use the [Azure Portal Cloud Shell](https://docs.microsoft.com/en-us/azure/cloud-shell/overview?view=azure-cli-latest) if you do not want to install the Azure CLI locally.

    Option 3: You can use a Docker container with the Azure CLI pre-installed.  Run the following command inside your shell (i.e., command prompt, Powershell, Bash, etc.):
      ```
      docker run -it -p 8001:8001 azuresdk/azure-cli-python:latest bash 
      ```
2. If you have not done so already, open a shell environment

3. Execute the following command to login to your Azure Subscription:
    
    ```
    az login -u <username> -p <password>
    ```
    A successful login will result in JSON output similar to the following:
    
    ```json
    [
       {
        "cloudName": "AzureCloud",
        "id": "6e7ce629-5859-4837-bce5-571fe7b268c5",
        "isDefault": false,
        "name": "MTC Houston Labs",
        "state": "Enabled",
        "tenantId": "a8e59e50-6360-4372-a629-9a9bf465158e",
        "user": {
          "name": "ratella@mtchouston.net",
          "type": "user"
        }
      }
    ]
    
    ```
    > Note:  If your AAD policy requires multi-factor authentication, you will need to execute the following command:
    > ```
    > az login
    > ```
    > You will receive a token that you must then authenticate your device with at [http://aka.ms/devicelogin](http://aka.ms/devicelogin).  It is recommended to access that Url with a InPrivate/InCognito browser session to avoid cookie conflicts.
    Then use following command to list your subscriptions to get the subscrition id for the desired subscription to deploy your lab resources:
    > ```
    > az account list
    > ```
4. All the labs assume you will be using the default subscription associated with your Azure login Id.  If you want to change subscriptions, find the **"id"** value from the appropriate subscription in the returned json or the account list and execute the following command:
    ```
    az account set --subscription="<SUBSCRIPTION_ID>"

Now go back and continue with your lab.
